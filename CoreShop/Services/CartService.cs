using CoreShop.CORE.Service;
using CoreShop.MODEL.Entities;
using CoreShop.Models;
using System.Text.Json;

namespace CoreShop.Services
{
    /// <summary>
    /// Single owner of the session-backed shopping cart. No other class
    /// reads or writes the cart session entry.
    /// </summary>
    public class CartService : ICartService
    {
        private const string CartSessionKey = "Cart";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICoreService<Product> _productService;

        public CartService(IHttpContextAccessor httpContextAccessor, ICoreService<Product> productService)
        {
            _httpContextAccessor = httpContextAccessor;
            _productService = productService;
        }

        private ISession Session =>
            _httpContextAccessor.HttpContext?.Session
            ?? throw new InvalidOperationException("The cart requires an active HTTP session.");

        public IReadOnlyList<CartItem> GetItems() => Load();

        public int GetItemCount() => Load().Sum(x => x.Quantity);

        public decimal GetTotalPrice() => Load().Sum(x => x.ProductPrice * x.Quantity);

        public CartOperationResult AddItem(int productId)
        {
            var product = _productService.GetById(productId);

            if (product == null)
                return CartOperationResult.ProductNotFound;

            if (product.ProductStock <= 0)
                return CartOperationResult.OutOfStock;

            var cart = Load();
            var existingItem = cart.FirstOrDefault(x => x.ProductId == productId);

            if (existingItem == null)
            {
                cart.Add(new CartItem
                {
                    ProductId = product.ID,
                    ProductName = product.ProductName,
                    ProductImage = product.ProductImage,
                    ProductPrice = product.ProductPrice,
                    Quantity = 1
                });
            }
            else if (existingItem.Quantity < product.ProductStock)
            {
                existingItem.Quantity++;
            }
            else
            {
                return CartOperationResult.StockLimitReached;
            }

            Save(cart);
            return CartOperationResult.Success;
        }

        public CartOperationResult IncreaseQuantity(int productId)
        {
            var cart = Load();
            var item = cart.FirstOrDefault(x => x.ProductId == productId);

            if (item == null)
                return CartOperationResult.ProductNotFound;

            var product = _productService.GetById(productId);

            if (product == null)
                return CartOperationResult.ProductNotFound;

            if (item.Quantity >= product.ProductStock)
                return CartOperationResult.StockLimitReached;

            item.Quantity++;
            Save(cart);
            return CartOperationResult.Success;
        }

        public void DecreaseQuantity(int productId)
        {
            var cart = Load();
            var item = cart.FirstOrDefault(x => x.ProductId == productId);

            if (item == null)
                return;

            item.Quantity--;

            if (item.Quantity <= 0)
                cart.Remove(item);

            Save(cart);
        }

        public void RemoveItem(int productId)
        {
            var cart = Load();
            var item = cart.FirstOrDefault(x => x.ProductId == productId);

            if (item == null)
                return;

            cart.Remove(item);
            Save(cart);
        }

        public void Clear() => Session.Remove(CartSessionKey);

        private List<CartItem> Load()
        {
            var cartJson = Session.GetString(CartSessionKey);

            if (string.IsNullOrEmpty(cartJson))
                return new List<CartItem>();

            return JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }

        private void Save(List<CartItem> cart) =>
            Session.SetString(CartSessionKey, JsonSerializer.Serialize(cart));
    }
}
