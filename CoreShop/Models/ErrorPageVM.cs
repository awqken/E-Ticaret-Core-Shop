namespace CoreShop.Models
{
    public class ErrorPageVM
    {
        public int StatusCode { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public string IconCssClass { get; init; } = string.Empty;
    }
}
