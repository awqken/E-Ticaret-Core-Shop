using System.ComponentModel.DataAnnotations;

namespace CoreShop.Areas.Models
{
    public class ProductFormVM
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [StringLength(120, ErrorMessage = "Ürün adı en fazla 120 karakter olabilir.")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Marka zorunludur.")]
        [StringLength(60, ErrorMessage = "Marka en fazla 60 karakter olabilir.")]
        public string ProductBrand { get; set; } = string.Empty;

        [Range(0.01, 10_000_000, ErrorMessage = "Fiyat 0'dan büyük olmalıdır.")]
        public decimal ProductPrice { get; set; }

        [Range(0, 1_000_000, ErrorMessage = "Stok negatif olamaz.")]
        public int ProductStock { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Kategori seçiniz.")]
        public int CategoryId { get; set; }

        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
        public string? Description { get; set; }

        /// <summary>Current stored image path; kept so update keeps the image when no new file is chosen.</summary>
        public string? ProductImage { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
