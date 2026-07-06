using CoreShop.Areas.Models;
using CoreShop.CORE.Service;
using CoreShop.MODEL.Constants;
using CoreShop.MODEL.Entities;
using CoreShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreShop.Areas.Admin.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ICoreService<Product> _productService;
        private readonly ICoreService<Category> _categoryService;
        private readonly IImageUploadService _imageUploadService;

        public ProductController(
            ICoreService<Product> productService,
            ICoreService<Category> categoryService,
            IImageUploadService imageUploadService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _imageUploadService = imageUploadService;
        }

        public IActionResult List(string sort, string search)
        {
            var products = _productService.GetAll();

            switch (sort)
            {
                case "new":
                    products = products.OrderByDescending(x => x.ID).ToList();
                    break;

                case "price_asc":
                    products = products.OrderBy(x => x.ProductPrice).ToList();
                    break;

                case "price_desc":
                    products = products.OrderByDescending(x => x.ProductPrice).ToList();
                    break;

                case "stock_desc":
                    products = products.OrderByDescending(x => x.ProductStock).ToList();
                    break;

                case "stock_asc":
                    products = products.OrderBy(x => x.ProductStock).ToList();
                    break;

                case "low_stock":
                    products = products
                        .Where(x => x.ProductStock > 0 && x.ProductStock <= 5)
                        .OrderBy(x => x.ProductStock)
                        .ToList();
                    break;

                case "out_stock":
                    products = products
                        .Where(x => x.ProductStock == 0)
                        .ToList();
                    break;

                default:
                    products = products.OrderByDescending(x => x.ID).ToList();
                    break;
            }

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();

                var matchedCategoryIds = _categoryService.GetAll()
                    .Where(x => x.CategoryName.ToLower().Contains(search))
                    .Select(x => x.ID)
                    .ToList();

                products = products.Where(x =>
                    x.ProductName.ToLower().Contains(search) ||
                    x.ProductBrand.ToLower().Contains(search) ||
                    (x.Description != null && x.Description.ToLower().Contains(search)) ||
                    matchedCategoryIds.Contains(x.CategoryId)
                ).ToList();
            }

            ViewBag.Sort = sort;
            ViewBag.Search = search;
            ViewBag.Categories = _categoryService.GetAll();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = GetCategoryOptions();
            return View(new ProductFormVM());
        }

        [HttpPost]
        public IActionResult Create(ProductFormVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = GetCategoryOptions();
                return View(model);
            }

            if (!TryApplyUploadedImage(model))
            {
                ViewBag.Categories = GetCategoryOptions();
                return View(model);
            }

            _productService.Create(new Product
            {
                ProductName = model.ProductName.Trim(),
                ProductBrand = model.ProductBrand.Trim(),
                ProductPrice = model.ProductPrice,
                ProductStock = model.ProductStock,
                CategoryId = model.CategoryId,
                Description = model.Description?.Trim(),
                ProductImage = model.ProductImage
            });

            TempData["Success"] = "Ürün eklendi.";
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var product = _productService.GetById(id);

            if (product == null)
            {
                return RedirectToAction("List");
            }

            ViewBag.Categories = GetCategoryOptions();

            return View(new ProductFormVM
            {
                ID = product.ID,
                ProductName = product.ProductName,
                ProductBrand = product.ProductBrand,
                ProductPrice = product.ProductPrice,
                ProductStock = product.ProductStock,
                CategoryId = product.CategoryId,
                Description = product.Description,
                ProductImage = product.ProductImage
            });
        }

        [HttpPost]
        public IActionResult Update(ProductFormVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = GetCategoryOptions();
                return View(model);
            }

            var existingProduct = _productService.GetById(model.ID);

            if (existingProduct == null)
            {
                return RedirectToAction("List");
            }

            var previousImage = existingProduct.ProductImage;

            if (!TryApplyUploadedImage(model))
            {
                ViewBag.Categories = GetCategoryOptions();
                return View(model);
            }

            existingProduct.ProductName = model.ProductName.Trim();
            existingProduct.ProductBrand = model.ProductBrand.Trim();
            existingProduct.ProductPrice = model.ProductPrice;
            existingProduct.ProductStock = model.ProductStock;
            existingProduct.CategoryId = model.CategoryId;
            existingProduct.Description = model.Description?.Trim();
            existingProduct.ProductImage = model.ProductImage;

            _productService.Update(existingProduct);

            // A newly uploaded file replaced the old one; clean up the orphan.
            if (previousImage != existingProduct.ProductImage)
                _imageUploadService.DeleteProductImage(previousImage);

            TempData["Success"] = "Ürün güncellendi.";
            return RedirectToAction("List");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = _productService.GetById(id);

            if (product != null)
            {
                _productService.Delete(product);
                _imageUploadService.DeleteProductImage(product.ProductImage);
                TempData["Success"] = "Ürün silindi.";
            }

            return RedirectToAction("List");
        }

        /// <summary>
        /// Stores the uploaded file (when present) and points the form model at it.
        /// On rejection adds a field-level error and returns false.
        /// </summary>
        private bool TryApplyUploadedImage(ProductFormVM model)
        {
            if (model.ImageFile == null || model.ImageFile.Length == 0)
                return true;

            if (_imageUploadService.TrySaveProductImage(model.ImageFile, out var imagePath, out var uploadError))
            {
                model.ProductImage = imagePath;
                return true;
            }

            ModelState.AddModelError(nameof(model.ImageFile), uploadError ?? "Görsel yüklenemedi.");
            return false;
        }

        private List<SelectListItem> GetCategoryOptions()
        {
            return _categoryService.GetAll()
                .Select(x => new SelectListItem
                {
                    Text = x.CategoryName,
                    Value = x.ID.ToString()
                }).ToList();
        }
    }
}
