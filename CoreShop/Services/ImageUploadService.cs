namespace CoreShop.Services
{
    public class ImageUploadService : IImageUploadService
    {
        // Whitelist: raster image formats only. SVG/HTML are deliberately
        // excluded because files under wwwroot are served as-is (stored XSS risk).
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSizeBytes = 2 * 1024 * 1024;

        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ImageUploadService> _logger;

        public ImageUploadService(IWebHostEnvironment environment, ILogger<ImageUploadService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public bool TrySaveProductImage(IFormFile file, out string? relativePath, out string? error)
        {
            relativePath = null;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                _logger.LogWarning("Product image rejected: disallowed extension {Extension}", extension);
                error = "Yalnızca JPG, PNG veya WebP görselleri yükleyebilirsiniz.";
                return false;
            }

            if (file.Length == 0)
            {
                _logger.LogWarning("Product image rejected: empty file");
                error = "Yüklenen dosya boş.";
                return false;
            }

            if (file.Length > MaxFileSizeBytes)
            {
                _logger.LogWarning("Product image rejected: {FileSize} bytes exceeds the {MaxFileSize} byte limit",
                    file.Length, MaxFileSizeBytes);
                error = "Görsel boyutu en fazla 2 MB olabilir.";
                return false;
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "products");
            Directory.CreateDirectory(uploadsFolder);

            // Always generate the file name server-side; never trust the client's name.
            var fileName = Guid.NewGuid() + extension;

            using (var stream = new FileStream(Path.Combine(uploadsFolder, fileName), FileMode.Create))
            {
                file.CopyTo(stream);
            }

            relativePath = "products/" + fileName;
            error = null;

            _logger.LogInformation("Product image saved as {RelativePath}", relativePath);

            return true;
        }
    }
}
