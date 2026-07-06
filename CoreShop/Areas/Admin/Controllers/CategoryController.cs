using CoreShop.Areas.Models;
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
            return View(new CategoryFormVM());
        }

        [HttpPost]
        public IActionResult Create(CategoryFormVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _categoryService.Create(new Category
            {
                CategoryName = model.CategoryName.Trim(),
                Description = model.Description?.Trim()
            });

            TempData["Success"] = "Kategori eklendi.";
            return RedirectToAction("List");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _categoryService.GetById(id);

            if (category != null)
            {
                _categoryService.Delete(category);
                TempData["Success"] = "Kategori silindi.";
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

            return View(new CategoryFormVM
            {
                ID = category.ID,
                CategoryName = category.CategoryName,
                Description = category.Description
            });
        }

        [HttpPost]
        public IActionResult Update(CategoryFormVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingCategory = _categoryService.GetById(model.ID);

            if (existingCategory == null)
            {
                return RedirectToAction("List");
            }

            existingCategory.CategoryName = model.CategoryName.Trim();
            existingCategory.Description = model.Description?.Trim();

            _categoryService.Update(existingCategory);

            TempData["Success"] = "Kategori güncellendi.";
            return RedirectToAction("List");
        }
    }
}
