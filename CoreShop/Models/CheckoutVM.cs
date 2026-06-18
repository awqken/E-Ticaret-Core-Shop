using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CoreShop.Models
{
    public class CheckoutVM
    {
        [Required(ErrorMessage = "Ad Soyad zorunlu")]
        [RegularExpression(@"^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$", ErrorMessage = "Sadece harf giriniz")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Telefon zorunlu")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Şehir zorunlu")]
        public string City { get; set; }

        [Required(ErrorMessage = "İlçe zorunlu")]
        public string District { get; set; }

        [Required(ErrorMessage = "Adres zorunlu")]
        public string FullAddress { get; set; }

        [Required(ErrorMessage = "Kart ismi zorunlu")]
        [RegularExpression(@"^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$", ErrorMessage = "Sadece harf giriniz")]
        public string CardName { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Kart numarası 16 haneli olmalı")]
        public string CardNumber { get; set; }

        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "MM/YY formatında gir")]
        public string ExpireDate { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "CVV 3 haneli olmalı")]
        public string CVV { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
