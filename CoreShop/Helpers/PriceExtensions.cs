using System.Globalization;

namespace CoreShop.Helpers
{
    public static class PriceExtensions
    {
        private static readonly CultureInfo Turkish = CultureInfo.GetCultureInfo("tr-TR");

        /// <summary>Formats a price the Turkish way: 24.999 ₺ (decimals only when present).</summary>
        public static string ToTryPrice(this decimal value)
        {
            var format = value == decimal.Truncate(value) ? "N0" : "N2";
            return value.ToString(format, Turkish) + " ₺";
        }

        /// <summary>Rounded discount percentage from <paramref name="oldPrice"/> down to <paramref name="currentPrice"/>; 0 when there is no real discount.</summary>
        public static int DiscountPercentFrom(this decimal currentPrice, decimal? oldPrice)
        {
            if (!oldPrice.HasValue || oldPrice.Value <= currentPrice || oldPrice.Value <= 0)
                return 0;

            return (int)Math.Round((1 - currentPrice / oldPrice.Value) * 100);
        }
    }
}
