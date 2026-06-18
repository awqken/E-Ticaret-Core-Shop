using CoreShop.Areas.Models;
using CoreShop.CORE.Service;
using CoreShop.MODEL.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ICoreService<Order> _orderService;
        private readonly ICoreService<Category> _categoryService;
        private readonly ICoreService<Product> _productService;
        private readonly ICoreService<User> _userService;

        public HomeController(ICoreService<Order> odb, ICoreService<Category> cdb, ICoreService<Product> pdb, ICoreService<User> udb)
        {
            _orderService    = odb;
            _categoryService = cdb;
            _productService  = pdb;
            _userService     = udb;
        }

        public IActionResult Index()
        {
            var orders   = _orderService.GetAll();
            var products = _productService.GetAll();
            var lowStock = products
                .Where(x => x.ProductStock <= 5)
                .OrderBy(x => x.ProductStock)
                .ToList();

            var model = new AdminDashboardVM
            {
                ProductCount  = products.Count(),
                CategoryCount = _categoryService.GetAll().Count(),
                OrderCount    = orders.Count(),
                UserCount     = _userService.GetAll().Count(),
                LowStockProducts = lowStock.Take(6).ToList(),
                LowStockCount    = lowStock.Count(),
                TotalRevenue  = orders.Where(x => x.Status != "Cancelled").Sum(x => x.TotalPrice),
                LastProducts  = products.OrderByDescending(x => x.ID).Take(5).ToList()
            };

            return View(model);
        }
    }
}
