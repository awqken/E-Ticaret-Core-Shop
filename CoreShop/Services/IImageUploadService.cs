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

        /// <summary>
        /// Deletes a previously uploaded product image so replaced or removed
        /// products do not leave orphan files behind. Only touches files this
        /// service created (GUID names); seed images shipped with the repo are
        /// never deleted because the in-memory data returns on restart.
        /// </summary>
        void DeleteProductImage(string? relativePath);
    }
}
