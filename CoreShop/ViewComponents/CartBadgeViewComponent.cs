using CoreShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreShop.ViewComponents
{
    /// <summary>Renders the cart item count badge shown in the navbar on every page.</summary>
    public class CartBadgeViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;

        public CartBadgeViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public IViewComponentResult Invoke() => View(_cartService.GetItemCount());
    }
}
