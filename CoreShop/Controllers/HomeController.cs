using CoreShop.CORE.Service;
using CoreShop.MODEL.Entities;
using CoreShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICoreService<Product> _productService;
        private readonly ICoreService<Category> _categoryService;

        public HomeController(ICoreService<Product> productService, ICoreService<Category> categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var model = new HomePageVM
            {
                Products = _productService.GetAll()
                                          .OrderByDescending(x => x.ID)
                                          .Take(8)
                                          .ToList(),

                Categories = _categoryService.GetAll()
                                             .OrderBy(x => x.CategoryName)
                                             .ToList()
            };

            return View(model);
        }
    }
}
