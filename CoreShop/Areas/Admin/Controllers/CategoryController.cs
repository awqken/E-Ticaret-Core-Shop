using CoreShop.CORE.Service;
using CoreShop.MODEL.Constants;
using CoreShop.MODEL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreShop.Areas.Admin.Controllers 
{
    [Authorize(Roles = UserRoles.Admin)]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICoreService<Category> _categoryService;

        public CategoryController(ICoreService<Category> categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult List()
        {
            var categories = _categoryService.GetAll();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            category.Products = null;

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _categoryService.Create(category);
            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _categoryService.GetById(id);

            if (category != null)
            {
                _categoryService.Delete(category);
            }

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var category = _categoryService.GetById(id);

            if (category == null)
            {
                return RedirectToAction("List");
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Update(Category category)
        {
            category.Products = null;

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _categoryService.Update(category);
            return RedirectToAction("List");
        }
    }
}