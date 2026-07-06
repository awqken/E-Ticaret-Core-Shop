using CoreShop.Models;

namespace CoreShop.Services
{
    public interface ICheckoutService
    {
        /// <summary>
        /// Places an order for the current cart: validates the (simulated) payment,
        /// revalidates stock, creates the order with its details, decrements stock
        /// and clears the cart. Fails as a whole before any data is written.
        /// </summary>
        CheckoutResult PlaceOrder(int userId, CheckoutVM model);
    }
}
