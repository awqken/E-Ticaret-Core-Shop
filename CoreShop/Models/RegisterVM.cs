using System.ComponentModel.DataAnnotations;

namespace CoreShop.Models
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [StringLength(80, ErrorMessage = "Ad Soyad en fazla 80 karakter olabilir.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
        [StringLength(120, ErrorMessage = "E-posta en fazla 120 karakter olabilir.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [StringLength(64, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
