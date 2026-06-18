using CoreShop.CORE.Service;
using CoreShop.Helpers;
using CoreShop.MODEL.Entites;
using CoreShop.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoreShop.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICoreService<User> _udb;
        private readonly ICoreService<Order> _orderService;
        private readonly ICoreService<OrderDetail> _orderDetailService;

        public AccountController(
        ICoreService<User> udb,
        ICoreService<Order> orderService,
        ICoreService<OrderDetail> orderDetailService)
        {
            _udb = udb;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            password = PasswordHelper.Sha256Hash(password);

            var result = _udb.GetAll()
                .FirstOrDefault(x => x.Email == email && x.Password == password);

            if (result != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, result.FullName),
                    new Claim(ClaimTypes.Email, result.Email),
                    new Claim(ClaimTypes.Role, result.Role)
                };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal
                );

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "E-posta veya şifre hatalı.";
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            bool emailExists = _udb.GetAll().Any(x => x.Email == user.Email);

            if (emailExists)
            {
                ViewBag.Error = "Bu e-posta zaten kayıtlı.";
                return View();
            }

            user.Password = PasswordHelper.Sha256Hash(user.Password);
            user.Role = "User";

            _udb.Create(user);

            return RedirectToAction("Login");
        }

        [Authorize]
        public IActionResult Profile()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = _udb.GetAll().FirstOrDefault(x => x.Email == email);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var orders = _orderService.GetAll()
                .Where(x => x.UserId == user.ID)
                .OrderByDescending(x => x.ID)
                .ToList();

            var orderDetails = _orderDetailService.GetAll().ToList();

            var vm = new ProfileVM
            {
                User = user,
                Orders = orders,
                OrderDetails = orderDetails
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult UpdateProfile(User model)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            var user = _udb.GetAll().FirstOrDefault(x => x.Email == email);

            if (user == null)
                return RedirectToAction("Login");

            user.City = model.City;
            user.District = model.District;
            user.FullAddress = model.FullAddress;
            user.PhoneNumber = model.PhoneNumber;

            _udb.Update(user);

            TempData["Success"] = "Adres bilgileri güncellendi.";

            return RedirectToAction("Profile");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}