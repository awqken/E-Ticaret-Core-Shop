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
            return View(new LoginVM());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _userService.GetAll().FirstOrDefault(x => x.Email == model.Email);

            if (user != null)
            {
                var verification = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

                if (verification != PasswordVerificationResult.Failed)
                {
                    if (verification == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                        user.Password = _passwordHasher.HashPassword(user, model.Password);
                        _userService.Update(user);
                    }

                    await SignInAsync(user);

                    _logger.LogInformation("User {UserId} signed in", user.ID);

                    return RedirectToAction("Index", "Home");
                }
            }

            _logger.LogWarning("Failed login attempt for {Email}", model.Email);

            ModelState.AddModelError(string.Empty, "E-posta veya şifre hatalı.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterVM());
        }

        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool emailExists = _userService.GetAll().Any(x => x.Email == model.Email);

            if (emailExists)
            {
                ModelState.AddModelError(nameof(model.Email), "Bu e-posta zaten kayıtlı.");
                return View(model);
            }

            var user = new User
            {
                FullName = model.FullName.Trim(),
                Email = model.Email.Trim(),
                Role = UserRoles.Customer
            };

            user.Password = _passwordHasher.HashPassword(user, model.Password);

            _userService.Create(user);

            _logger.LogInformation("New user registered: {UserId}", user.ID);

            TempData["Success"] = "Hesabın oluşturuldu. Şimdi giriş yapabilirsin.";

            return RedirectToAction("Login");
        }

        [Authorize]
        public IActionResult Profile()
        {
            var user = GetCurrentUser();

            if (user == null)
                return RedirectToAction("Login");

            return View(BuildProfileVM(user));
        }

        [HttpPost]
        [Authorize]
        public IActionResult UpdateProfile(ProfileUpdateVM model)
        {
            var user = GetCurrentUser();

            if (user == null)
                return RedirectToAction("Login");

            if (!ModelState.IsValid)
            {
                var vm = BuildProfileVM(user);
                vm.AddressForm = model;
                vm.ShowAddressForm = true;
                return View("Profile", vm);
            }

            user.City = model.City.Trim();
            user.District = model.District.Trim();
            user.FullAddress = model.FullAddress.Trim();
            user.PhoneNumber = model.PhoneNumber.Trim();

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

        private User? GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            return _userService.GetAll().FirstOrDefault(x => x.Email == email);
        }

        private ProfileVM BuildProfileVM(User user)
        {
            var detailsByOrderId = _orderDetailService.GetAll()
                .ToLookup(x => x.OrderId);

            var orders = _orderService.GetAll()
                .Where(x => x.UserId == user.ID)
                .OrderByDescending(x => x.ID)
                .Select(order => new ProfileOrderVM
                {
                    Order = order,
                    Details = detailsByOrderId[order.ID].ToList()
                })
                .ToList();

            return new ProfileVM
            {
                User = user,
                Orders = orders,
                AddressForm = new ProfileUpdateVM
                {
                    City = user.City ?? string.Empty,
                    District = user.District ?? string.Empty,
                    FullAddress = user.FullAddress ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty
                }
            };
        }

        private async Task SignInAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }
    }
}
