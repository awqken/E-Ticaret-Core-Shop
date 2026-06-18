using CoreShop.CORE.Service;
using CoreShop.MODEL.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ICoreService<Product> _productService;
        private readonly ICoreService<Category> _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(
            ICoreService<Product> productService,
            ICoreService<Category> categoryService,
            IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult List(string sort,string search)
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

                var categories = _categoryService.GetAll();

                var matchedCategoryIds = categories
                    .Where(x => x.CategoryName.ToLower().Contains(search))
                    .Select(x => x.ID)
                    .ToList();

                products = products.Where(x =>
                    x.ProductName.ToLower().Contains(search) ||
                    x.ProductBrand.ToLower().Contains(search) ||
                    x.Description.ToLower().Contains(search) ||
                    matchedCategoryIds.Contains(x.CategoryId)
                ).ToList();
            }
            ViewBag.Sort = sort;
            ViewBag.Categories = _categoryService.GetAll();
            return View(products);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _categoryService.GetAll()
                .Select(x => new SelectListItem
                {
                    Text = x.CategoryName,
                    Value = x.ID.ToString()
                }).ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                product.ProductImage = "products/" + fileName;
            }

            _productService.Create(product);
            return RedirectToAction("List");
        }
        public IActionResult Delete(int id)
        {
            var product = _productService.GetById(id);

            if (product != null)
            {
                _productService.Delete(product);
            }

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

            ViewBag.Categories = _categoryService.GetAll()
                .Select(x => new SelectListItem
                {
                    Text = x.CategoryName,
                    Value = x.ID.ToString()
                }).ToList();

            return View(product);
        }

        [HttpPost]
        public IActionResult Update(Product product, IFormFile ImageFile)
        {
            ViewBag.Categories = _categoryService.GetAll()
                .Select(x => new SelectListItem
                {
                    Text = x.CategoryName,
                    Value = x.ID.ToString()
                }).ToList();

            ModelState.Remove("Category");
            ModelState.Remove("ImageFile");

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            var existingProduct = _productService.GetById(product.ID);

            if (existingProduct == null)
            {
                return RedirectToAction("List");
            }

            existingProduct.ProductName = product.ProductName;
            existingProduct.ProductBrand = product.ProductBrand;
            existingProduct.ProductPrice = product.ProductPrice;
            existingProduct.ProductStock = product.ProductStock;
            existingProduct.Description = product.Description;
            existingProduct.CategoryId = product.CategoryId;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                existingProduct.ProductImage = "products/" + fileName;
            }

            _productService.Update(existingProduct);
            return RedirectToAction("List");
        }
    }
}