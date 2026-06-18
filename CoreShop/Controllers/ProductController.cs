using CoreShop.CORE.Service;
using CoreShop.MODEL.Entites;
using CoreShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICoreService<Product> _productService;
        private readonly ICoreService<Category> _categoryService;

        public ProductController(ICoreService<Product> productService, ICoreService<Category> categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Index(int? categoryIdSingle,List<int> categoryId,string search,List<string> brands,decimal? minPrice,decimal? maxPrice,string sort)
        {
            var allProducts = _productService.GetAll();
            var products = allProducts;

            if (categoryIdSingle.HasValue)
            {
                categoryId = new List<int> { categoryIdSingle.Value };
            }

            if (categoryId != null && categoryId.Any())
            {
                products = products.Where(x => categoryId.Contains(x.CategoryId)).ToList();
            }

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();

                var matchedCategoryIds = _categoryService.GetAll()
                    .Where(x => x.CategoryName.ToLower().Contains(search))
                    .Select(x => x.ID)
                    .ToList();

                products = products.Where(x => x.ProductName.ToLower().Contains(search) || x.ProductBrand.ToLower().Contains(search) || matchedCategoryIds.Contains(x.CategoryId)).ToList();
            }

            if (brands != null && brands.Any())
            {
                products = products.Where(x => brands.Contains(x.ProductBrand)).ToList();
            }

            if (minPrice.HasValue)
            {
                products = products.Where(x => x.ProductPrice >= minPrice.Value).ToList();
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(x => x.ProductPrice <= maxPrice.Value).ToList();
            }

            switch (sort)
            {
                case "price_asc":
                    products = products.OrderBy(x => x.ProductPrice).ToList();
                    break;

                case "price_desc":
                    products = products.OrderByDescending(x => x.ProductPrice).ToList();
                    break;

                case "az":
                    products = products.OrderBy(x => x.ProductName).ToList();
                    break;

                case "za":
                    products = products.OrderByDescending(x => x.ProductName).ToList();
                    break;

                default:
                    products = products.OrderByDescending(x => x.ID).ToList();
                    break;
            }

            var model = new ProductPageVM
            {
                Products = products.ToList(), 
                Categories = _categoryService.GetAll().OrderBy(x => x.CategoryName).ToList(),
                Search = search,
                Sort = sort,

                Brands = allProducts
                    .Select(x => x.ProductBrand)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList(),

                SelectedCategoryIds = categoryId ?? new List<int>(),
                SelectedBrands = brands ?? new List<string>(),

                MinPrice = minPrice,
                MaxPrice = maxPrice
            };

            var cartJson = HttpContext.Session.GetString("Cart");

            if (!string.IsNullOrEmpty(cartJson))
            {
                var cart = System.Text.Json.JsonSerializer.Deserialize<List<CartItem>>(cartJson);
                ViewBag.CartCount = cart.Sum(x => x.Quantity);
            }
            else
            {
                ViewBag.CartCount = 0;
            }

            return View(model);
        }
        public IActionResult Detail(int id)
        {
            var product = _productService.GetById(id);

            if (product == null)
                return NotFound();

            var allProducts = _productService.GetAll().Where(x => x.ID != product.ID).ToList();

            var relatedProducts = allProducts
                .Where(x => x.CategoryId == product.CategoryId)
                .OrderBy(x => Guid.NewGuid())
                .Take(4)
                .ToList();

           
            if (relatedProducts.Count < 4)
            {
                var extraProducts = allProducts
                    .Where(x => !relatedProducts.Any(r => r.ID == x.ID))
                    .OrderBy(x => Guid.NewGuid())
                    .Take(4 - relatedProducts.Count)
                    .ToList();

                relatedProducts.AddRange(extraProducts);
            }

            var model = new CoreShop.Models.ProductDetailVM
            {
                Product = product,
                RelatedProducts = relatedProducts
            };

            var cartJson = HttpContext.Session.GetString("Cart");

            if (!string.IsNullOrEmpty(cartJson))
            {
                var cart = System.Text.Json.JsonSerializer.Deserialize<List<CoreShop.Models.CartItem>>(cartJson);
                ViewBag.CartCount = cart.Sum(x => x.Quantity);
            }
            else
            {
                ViewBag.CartCount = 0;
            }

            return View(model);
        }
    }
}