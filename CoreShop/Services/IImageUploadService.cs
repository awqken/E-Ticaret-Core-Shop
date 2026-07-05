namespace CoreShop.Services
{
    public interface IImageUploadService
    {
        /// <summary>
        /// Validates and stores a product image under wwwroot/images/products.
        /// Returns true with the web-relative path on success; false with a
        /// user-facing Turkish error message when the file is rejected.
        /// </summary>
        bool TrySaveProductImage(IFormFile file, out string? relativePath, out string? error);
    }
}
