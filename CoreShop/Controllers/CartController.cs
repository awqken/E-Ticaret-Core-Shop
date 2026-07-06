using CoreShop.CORE.Service;
using CoreShop.MODEL.Entities;
using CoreShop.Models;
using CoreShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoreShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ICheckoutService _checkoutService;
        private readonly ICoreService<User> _userService;

        public CartController(
            ICartService cartService,
            ICheckoutService checkoutService,
            ICoreService<User> userService)
        {
            _cartService = cartService;
            _checkoutService = checkoutService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View(_cartService.GetItems());
        }

        [HttpPost]
        public IActionResult AddToCart(int id, string? returnUrl)
        {
            var result = _cartService.AddItem(id);

            if (result == CartOperationResult.ProductNotFound)
                return NotFound();

            SetCartWarning(result);
            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            _cartService.RemoveItem(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Increase(int id)
        {
            SetCartWarning(_cartService.IncreaseQuantity(id));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Decrease(int id)
        {
            _cartService.DecreaseQuantity(id);
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            if (_cartService.GetItemCount() == 0)
                return RedirectToAction("Index");

            var user = GetCurrentUser();

            var model = new CheckoutVM();

            if (user != null)
            {
                model.FullName = user.FullName;
                model.PhoneNumber = user.PhoneNumber ?? string.Empty;
                model.City = user.City ?? string.Empty;
                model.District = user.District ?? string.Empty;
                model.FullAddress = user.FullAddress ?? string.Empty;
            }

            model.TotalPrice = _cartService.GetTotalPrice();

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(CheckoutVM model)
        {
            if (_cartService.GetItemCount() == 0)
                return RedirectToAction("Index");

            if (!ModelState.IsValid)
            {
                model.TotalPrice = _cartService.GetTotalPrice();
                return View(model);
            }

            var user = GetCurrentUser();

            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = _checkoutService.PlaceOrder(user.ID, model);

            if (!result.Succeeded)
            {
                if (result.Error == CheckoutError.EmptyCart)
                    return RedirectToAction("Index");

                ViewBag.Error = result.Error switch
                {
                    CheckoutError.InvalidCard => "Ödeme başarısız. Geçerli bir 16 haneli kart numarası girin.",
                    CheckoutError.InsufficientStock => $"\"{result.ProblemProductName}\" için yeterli stok kalmadı. Lütfen sepetinizi güncelleyin.",
                    _ => "Sipariş oluşturulamadı. Lütfen tekrar deneyin."
                };

                model.TotalPrice = _cartService.GetTotalPrice();
                return View(model);
            }

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }

        private User? GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            return _userService.GetAll().FirstOrDefault(x => x.Email == email);
        }

        private void SetCartWarning(CartOperationResult result)
        {
            var warning = result switch
            {
                CartOperationResult.OutOfStock => "Bu ürün stokta yok.",
                CartOperationResult.StockLimitReached => "Bu ürün için maksimum stok miktarına ulaştınız.",
                _ => null
            };

            if (warning != null)
                TempData["CartWarning"] = warning;
        }

        /// <summary>
        /// Redirects only to application-local URLs; anything else falls back
        /// to the cart page (open-redirect protection).
        /// </summary>
        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index");
        }
    }
}
