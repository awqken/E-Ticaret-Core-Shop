using System.ComponentModel.DataAnnotations;

namespace CoreShop.Areas.Models
{
    public class CategoryFormVM
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(60, ErrorMessage = "Kategori adı en fazla 60 karakter olabilir.")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Açıklama en fazla 200 karakter olabilir.")]
        public string? Description { get; set; }
    }
}
