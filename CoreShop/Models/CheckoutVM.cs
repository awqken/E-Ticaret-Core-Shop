using CoreShop.Models.Validation;
using System.ComponentModel.DataAnnotations;

namespace CoreShop.Models
{
    public class CheckoutVM
    {
        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [RegularExpression(@"^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$", ErrorMessage = "Ad Soyad yalnızca harf içerebilir.")]
        [StringLength(80, ErrorMessage = "Ad Soyad en fazla 80 karakter olabilir.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [RegularExpression(@"^0?5\d{9}$", ErrorMessage = "Geçerli bir cep telefonu girin (örn: 05001234567).")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şehir zorunludur.")]
        [StringLength(60, ErrorMessage = "Şehir en fazla 60 karakter olabilir.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "İlçe zorunludur.")]
        [StringLength(60, ErrorMessage = "İlçe en fazla 60 karakter olabilir.")]
        public string District { get; set; } = string.Empty;

        [Required(ErrorMessage = "Açık adres zorunludur.")]
        [StringLength(300, ErrorMessage = "Adres en fazla 300 karakter olabilir.")]
        public string FullAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kart üzerindeki isim zorunludur.")]
        [RegularExpression(@"^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$", ErrorMessage = "Kart ismi yalnızca harf içerebilir.")]
        [StringLength(80, ErrorMessage = "Kart ismi en fazla 80 karakter olabilir.")]
        public string CardName { get; set; } = string.Empty;

        // 16 digits, optionally separated by single spaces or dashes ("4242 4242 4242 4242").
        [Required(ErrorMessage = "Kart numarası zorunludur.")]
        [RegularExpression(@"^(?:\d[ -]?){15}\d$", ErrorMessage = "Kart numarası 16 haneli olmalıdır.")]
        public string CardNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Son kullanma tarihi zorunludur.")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Tarihi AA/YY formatında girin.")]
        [FutureExpiryDate(ErrorMessage = "Kartın son kullanma tarihi geçmiş.")]
        public string ExpireDate { get; set; } = string.Empty;

        [Required(ErrorMessage = "CVV zorunludur.")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV 3 haneli olmalıdır.")]
        public string CVV { get; set; } = string.Empty;

        public decimal TotalPrice { get; set; }

        /// <summary>Display-only: the cart lines shown in the order summary panel.</summary>
        public IReadOnlyList<CartItem> Items { get; set; } = Array.Empty<CartItem>();
    }
}
