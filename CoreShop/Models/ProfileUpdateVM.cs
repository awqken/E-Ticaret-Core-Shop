using System.ComponentModel.DataAnnotations;

namespace CoreShop.Models
{
    public class ProfileUpdateVM
    {
        [Required(ErrorMessage = "Şehir zorunludur.")]
        [StringLength(60, ErrorMessage = "Şehir en fazla 60 karakter olabilir.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "İlçe zorunludur.")]
        [StringLength(60, ErrorMessage = "İlçe en fazla 60 karakter olabilir.")]
        public string District { get; set; } = string.Empty;

        [Required(ErrorMessage = "Açık adres zorunludur.")]
        [StringLength(300, ErrorMessage = "Adres en fazla 300 karakter olabilir.")]
        public string FullAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [RegularExpression(@"^0?5\d{9}$", ErrorMessage = "Geçerli bir cep telefonu girin (örn: 05001234567).")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
