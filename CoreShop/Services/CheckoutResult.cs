using CoreShop.MODEL.Entities;

namespace CoreShop.Services
{
    public enum CheckoutError
    {
        None,
        EmptyCart,
        InvalidCard,
        InsufficientStock
    }

    public class CheckoutResult
    {
        public bool Succeeded { get; private init; }
        public Order? Order { get; private init; }
        public CheckoutError Error { get; private init; }

        /// <summary>Set when <see cref="Error"/> is <see cref="CheckoutError.InsufficientStock"/>.</summary>
        public string? ProblemProductName { get; private init; }

        public static CheckoutResult Success(Order order) =>
            new() { Succeeded = true, Order = order, Error = CheckoutError.None };

        public static CheckoutResult Failure(CheckoutError error, string? problemProductName = null) =>
            new() { Succeeded = false, Error = error, ProblemProductName = problemProductName };
    }
}
