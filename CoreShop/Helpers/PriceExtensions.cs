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
    }
}
