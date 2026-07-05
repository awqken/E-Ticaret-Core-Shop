using System.ComponentModel.DataAnnotations;

namespace CoreShop.Models
{
    public class CheckoutVM
    {
        [Required(ErrorMessage = "Ad Soyad zorunlu")]
        [RegularExpression(@"^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$", ErrorMessage = "Sadece harf giriniz")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon zorunlu")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şehir zorunlu")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "İlçe zorunlu")]
        public string District { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adres zorunlu")]
        public string FullAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kart ismi zorunlu")]
        [RegularExpression(@"^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$", ErrorMessage = "Sadece harf giriniz")]
        public string CardName { get; set; } = string.Empty;

        [Required]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Kart numarası 16 haneli olmalı")]
        public string CardNumber { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "MM/YY formatında gir")]
        public string ExpireDate { get; set; } = string.Empty;

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "CVV 3 haneli olmalı")]
        public string CVV { get; set; } = string.Empty;

        public decimal TotalPrice { get; set; }
    }
}
