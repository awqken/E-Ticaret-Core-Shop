using CoreShop.CORE.Service;
using CoreShop.MODEL.Entities;
using CoreShop.MODEL.Enums;
using CoreShop.Models;

namespace CoreShop.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartService _cartService;
        private readonly ICoreService<Product> _productService;
        private readonly ICoreService<Order> _orderService;
        private readonly ICoreService<OrderDetail> _orderDetailService;
        private readonly ILogger<CheckoutService> _logger;

        public CheckoutService(
            ICartService cartService,
            ICoreService<Product> productService,
            ICoreService<Order> orderService,
            ICoreService<OrderDetail> orderDetailService,
            ILogger<CheckoutService> logger)
        {
            _cartService = cartService;
            _productService = productService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _logger = logger;
        }

        public CheckoutResult PlaceOrder(int userId, CheckoutVM model)
        {
            var cart = _cartService.GetItems();

            if (cart.Count == 0)
                return CheckoutResult.Failure(CheckoutError.EmptyCart);

            // Simulated payment: the card number must contain exactly 16 digits.
            var cardDigits = model.CardNumber.Replace(" ", "").Replace("-", "");
            if (cardDigits.Length != 16 || !cardDigits.All(char.IsDigit))
            {
                _logger.LogWarning("Checkout payment validation failed for user {UserId}", userId);
                return CheckoutResult.Failure(CheckoutError.InvalidCard);
            }

            // Revalidate stock before writing anything: the cart may be stale.
            foreach (var item in cart)
            {
                var product = _productService.GetById(item.ProductId);

                if (product == null || product.ProductStock < item.Quantity)
                {
                    _logger.LogWarning(
                        "Checkout blocked for user {UserId}: insufficient stock for product {ProductId}",
                        userId, item.ProductId);
                    return CheckoutResult.Failure(CheckoutError.InsufficientStock, item.ProductName);
                }
            }

            var order = _orderService.Create(new Order
            {
                UserId = userId,
                TotalPrice = cart.Sum(x => x.ProductPrice * x.Quantity),
                Status = OrderStatus.Paid,

                CardName = model.CardName,
                CardLast4 = cardDigits.Substring(cardDigits.Length - 4),

                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                City = model.City,
                District = model.District,
                FullAddress = model.FullAddress
            });

            foreach (var item in cart)
            {
                _orderDetailService.Create(new OrderDetail
                {
                    OrderId = order.ID,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.ProductPrice,
                    ProductName = item.ProductName,
                    ProductImage = item.ProductImage
                });

                var product = _productService.GetById(item.ProductId);

                if (product != null)
                {
                    product.ProductStock -= item.Quantity;
                    _productService.Update(product);
                }
            }

            _cartService.Clear();

            _logger.LogInformation(
                "Order {OrderId} created for user {UserId}: {ItemCount} items, total {TotalPrice}",
                order.ID, userId, cart.Sum(x => x.Quantity), order.TotalPrice);

            return CheckoutResult.Success(order);
        }
    }
}
