using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CoreShop.Models.Validation
{
    /// <summary>
    /// Validates an MM/YY card expiry: the card must not be expired.
    /// Format validation is left to a preceding <see cref="RegularExpressionAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class FutureExpiryDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is not string text || string.IsNullOrWhiteSpace(text))
                return true; // [Required] handles absence

            if (!DateTime.TryParseExact(text, "MM/yy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var parsed))
                return true; // format errors are reported by the regex attribute

            // A card is valid through the last day of its expiry month.
            var endOfExpiryMonth = new DateTime(parsed.Year, parsed.Month, 1)
                .AddMonths(1)
                .AddDays(-1);

            return endOfExpiryMonth >= DateTime.Today;
        }
    }
}
