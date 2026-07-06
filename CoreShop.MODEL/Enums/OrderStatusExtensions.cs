namespace CoreShop.MODEL.Enums
{
    public static class OrderStatusExtensions
    {
        public static string ToDisplayName(this OrderStatus status) => status switch
        {
            OrderStatus.Pending => "Ödeme Bekliyor",
            OrderStatus.Paid => "Ödendi",
            OrderStatus.Preparing => "Hazırlanıyor",
            OrderStatus.Shipped => "Kargoda",
            OrderStatus.Delivered => "Teslim Edildi",
            OrderStatus.Cancelled => "İptal Edildi",
            _ => status.ToString()
        };

        /// <summary>Final statuses can never change again.</summary>
        public static bool IsFinal(this OrderStatus status) =>
            status is OrderStatus.Delivered or OrderStatus.Cancelled;

        /// <summary>
        /// The order lifecycle: Pending → Paid → Preparing → Shipped → Delivered,
        /// with cancellation possible until the order has shipped.
        /// </summary>
        public static IReadOnlyList<OrderStatus> GetAllowedTransitions(this OrderStatus status) => status switch
        {
            OrderStatus.Pending => new[] { OrderStatus.Paid, OrderStatus.Cancelled },
            OrderStatus.Paid => new[] { OrderStatus.Preparing, OrderStatus.Cancelled },
            OrderStatus.Preparing => new[] { OrderStatus.Shipped, OrderStatus.Cancelled },
            OrderStatus.Shipped => new[] { OrderStatus.Delivered },
            _ => Array.Empty<OrderStatus>()
        };

        public static bool CanTransitionTo(this OrderStatus current, OrderStatus target) =>
            current.GetAllowedTransitions().Contains(target);
    }
}
