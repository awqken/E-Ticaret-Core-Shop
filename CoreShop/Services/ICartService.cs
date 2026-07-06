using CoreShop.Models;

namespace CoreShop.Services
{
    public interface ICartService
    {
        IReadOnlyList<CartItem> GetItems();
        int GetItemCount();
        decimal GetTotalPrice();
        CartOperationResult AddItem(int productId);
        CartOperationResult IncreaseQuantity(int productId);
        void DecreaseQuantity(int productId);
        void RemoveItem(int productId);
        void Clear();
    }
}
