using CoreShop.CORE.Service;
using CoreShop.MODEL.Entities;
using CoreShop.Models;
using CoreShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICoreService<Product> _productService;
        private readonly ICoreService<Category> _categoryService;
        private readonly ICoreService<OrderDetail> _orderDetailService;
        private readonly IHeroCampaignProvider _heroCampaigns;

        public HomeController(
            ICoreService<Product> productService,
            ICoreService<Category> categoryService,
            ICoreService<OrderDetail> orderDetailService,
            IHeroCampaignProvider heroCampaigns)
        {
            _productService = productService;
            _categoryService = categoryService;
            _orderDetailService = orderDetailService;
            _heroCampaigns = heroCampaigns;
        }

        // The optional key (/?campaign=gpus) previews a specific campaign;
        // visitors otherwise get the one drawn at application startup.
        public IActionResult Index(string? campaign = null)
        {
            var heroCampaign = _heroCampaigns.Find(campaign) ?? _heroCampaigns.Current;
            var products = _productService.GetAll();

            var bestSellerIds = _orderDetailService.GetAll()
                .GroupBy(x => x.ProductId)
                .OrderByDescending(g => g.Sum(x => x.Quantity))
                .Select(g => g.Key)
                .Take(4)
                .ToList();

            var model = new HomePageVM
            {
                FeaturedProducts = products
                    .Where(x => x.OldPrice.HasValue && x.OldPrice > x.ProductPrice)
                    .OrderByDescending(x => x.ProductStock > 0)
                    .ThenByDescending(x => x.ID)
                    .Take(8)
                    .ToList(),

                BestSellers = bestSellerIds
                    .Select(id => products.FirstOrDefault(p => p.ID == id))
                    .Where(p => p != null)
                    .Select(p => p!)
                    .ToList(),

                NewArrivals = products
                    .OrderByDescending(x => x.ID)
                    .Take(4)
                    .ToList(),

                Categories = _categoryService.GetAll()
                    .OrderBy(x => x.CategoryName)
                    .ToList(),

                Campaign = heroCampaign,

                HeroProduct = products.FirstOrDefault(x => x.ID == heroCampaign.FeaturedProductId)
            };

            return View(model);
        }
    }
}
