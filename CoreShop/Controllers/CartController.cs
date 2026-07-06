using CoreShop.CORE.Service;
using CoreShop.MODEL.Entities;
using CoreShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace CoreShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ICoreService<Product> _productService;
        private readonly ICoreService<Order> _orderService;
        private readonly ICoreService<OrderDetail> _orderDetailService;
        private readonly ICoreService<User> _userService;
        private readonly ILogger<CartController> _logger;

        public CartController(
        ICoreService<Product> productService,
        ICoreService<Order> orderService,
        ICoreService<OrderDetail> orderDetailService,
        ICoreService<User> userService,
        ILogger<CartController> logger)
        {
            _productService = productService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _userService = userService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            ViewBag.CartCount = cart.Sum(x => x.Quantity);
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int id, string? returnUrl)
        {
            var product = _productService.GetById(id);

            if (product == null)
                return NotFound();

            if (product.ProductStock <= 0)
            {
                TempData["CartWarning"] = "Bu ürün stokta yok.";
                return RedirectToLocal(returnUrl);
            }

            var cart = GetCart();

            var existingItem = cart.FirstOrDefault(x => x.ProductId == id);

            if (existingItem != null)
            {
                if (existingItem.Quantity < product.ProductStock)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    TempData["CartWarning"] = "Bu ürün için maksimum stok miktarına ulaştınız.";
                }
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.ID,
                    ProductName = product.ProductName,
                    ProductImage = product.ProductImage,
                    ProductPrice = product.ProductPrice,
                    Quantity = 1
                });
            }

            SaveCart(cart);

            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                cart.Remove(item);
            }

            SaveCart(cart);

            return RedirectToAction("Index");
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

        private List<CartItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");

            if (string.IsNullOrEmpty(cartJson))
                return new List<CartItem>();

            return JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("Cart", cartJson);
        }
        [HttpPost]
        public IActionResult Increase(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                var product = _productService.GetById(id);

                if (product != null)
                {
                    if (item.Quantity < product.ProductStock)
                    {
                        item.Quantity++;
                    }
                    else
                    {
                        TempData["CartWarning"] = "Bu ürün için maksimum stok miktarına ulaştınız.";
                    }
                }
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Decrease(int id)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                item.Quantity--;

                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }
            }

            SaveCart(cart);

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = GetCart();

            if (cart == null || !cart.Any())
                return RedirectToAction("Index");

            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = _userService.GetAll()
                .FirstOrDefault(x => x.Email == email);

            var model = new CheckoutVM();

            if (user != null)
            {
                model.FullName = user.FullName;
                model.PhoneNumber = user.PhoneNumber ?? string.Empty;
                model.City = user.City ?? string.Empty;
                model.District = user.District ?? string.Empty;
                model.FullAddress = user.FullAddress ?? string.Empty;
            }

            model.TotalPrice = cart.Sum(x => x.ProductPrice * x.Quantity);

            return View(model);
        }



        [Authorize]
        [HttpPost]
        public IActionResult Checkout(CheckoutVM model)
        {
            var cart = GetCart();

            if (cart == null || !cart.Any())
                return RedirectToAction("Index");

            if (!ModelState.IsValid)
            {
                model.TotalPrice = cart.Sum(x => x.ProductPrice * x.Quantity);
                return View(model);
            }

            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = _userService.GetAll()
                .FirstOrDefault(x => x.Email == email);

            if (user == null)
                return RedirectToAction("Login", "Account");

            decimal totalPrice = cart.Sum(x => x.ProductPrice * x.Quantity);

            // Simulate payment: card number must contain exactly 16 digits
            var cleanCard = model.CardNumber.Replace(" ", "").Replace("-", "");
            if (cleanCard.Length != 16 || !cleanCard.All(char.IsDigit))
            {
                _logger.LogWarning("Checkout payment validation failed for user {UserId}", user.ID);

                ViewBag.Error = "Ödeme başarısız. Geçerli bir 16 haneli kart numarası girin.";
                model.TotalPrice = totalPrice;
                return View(model);
            }

            var order = new Order
            {
                UserId = user.ID,
                TotalPrice = totalPrice,
                Status = "Paid",

                CardName = model.CardName,
                CardLast4 = cleanCard.Substring(cleanCard.Length - 4),

                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                City = model.City,
                District = model.District,
                FullAddress = model.FullAddress
            };

            _orderService.Create(order);

            var orderId = _orderService.GetAll()
                .OrderByDescending(x => x.ID)
                .First()
                .ID;

            foreach (var item in cart)
            {
                var detail = new OrderDetail
                {
                    OrderId = orderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.ProductPrice,
                    ProductName = item.ProductName,
                    ProductImage = item.ProductImage
                };

                _orderDetailService.Create(detail);
            }

            foreach (var item in cart)
            {
                var product = _productService.GetById(item.ProductId);

                if (product != null)
                {
                    product.ProductStock -= item.Quantity;

                    if (product.ProductStock < 0)
                        product.ProductStock = 0;

                    _productService.Update(product);
                }
            }

            HttpContext.Session.Remove("Cart");

            _logger.LogInformation(
                "Order {OrderId} created for user {UserId}: {ItemCount} items, total {TotalPrice}",
                orderId, user.ID, cart.Sum(x => x.Quantity), totalPrice);

            return RedirectToAction("Success");
        }



        public IActionResult Success()
        {
            return View();
        }
    }
}