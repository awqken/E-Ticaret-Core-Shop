using CoreShop.CORE.Service;
using CoreShop.MODEL.Constants;
using CoreShop.MODEL.Entities;
using CoreShop.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoreShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICoreService<User> _userService;
        private readonly ICoreService<Order> _orderService;
        private readonly ICoreService<OrderDetail> _orderDetailService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            ICoreService<User> userService,
            ICoreService<Order> orderService,
            ICoreService<OrderDetail> orderDetailService,
            IPasswordHasher<User> passwordHasher,
            ILogger<AccountController> logger)
        {
            _userService = userService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "E-posta ve şifre alanları zorunludur.";
                return View();
            }

            var user = _userService.GetAll().FirstOrDefault(x => x.Email == email);

            if (user != null)
            {
                var verification = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

                if (verification != PasswordVerificationResult.Failed)
                {
                    if (verification == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                        user.Password = _passwordHasher.HashPassword(user, password);
                        _userService.Update(user);
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.FullName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role)
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

                    _logger.LogInformation("User {UserId} signed in", user.ID);

                    return RedirectToAction("Index", "Home");
                }
            }

            _logger.LogWarning("Failed login attempt for {Email}", email);

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
            if (string.IsNullOrWhiteSpace(user.FullName) ||
                string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.Password))
            {
                ViewBag.Error = "Ad Soyad, e-posta ve şifre alanları zorunludur.";
                return View();
            }

            bool emailExists = _userService.GetAll().Any(x => x.Email == user.Email);

            if (emailExists)
            {
                ViewBag.Error = "Bu e-posta zaten kayıtlı.";
                return View();
            }

            user.Password = _passwordHasher.HashPassword(user, user.Password);
            user.Role = UserRoles.Customer;

            _userService.Create(user);

            _logger.LogInformation("New user registered: {UserId}", user.ID);

            return RedirectToAction("Login");
        }

        [Authorize]
        public IActionResult Profile()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = _userService.GetAll().FirstOrDefault(x => x.Email == email);

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
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = _userService.GetAll().FirstOrDefault(x => x.Email == email);

            if (user == null)
                return RedirectToAction("Login");

            user.City = model.City;
            user.District = model.District;
            user.FullAddress = model.FullAddress;
            user.PhoneNumber = model.PhoneNumber;

            _userService.Update(user);

            TempData["Success"] = "Adres bilgileri güncellendi.";

            return RedirectToAction("Profile");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}
