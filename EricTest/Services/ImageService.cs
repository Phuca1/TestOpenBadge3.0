namespace EricTest.Services
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _env;
        private const string ImageFolder = "badge-images";

        public ImageService(IWebHostEnvironment env)
        {
            _env = env;
            EnsureImageDirectoryExists();
        }

        public async Task<string> SaveImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("No image file provided");

            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!new[] { ".png", ".jpg", ".jpeg", ".svg" }.Contains(extension))
                throw new ArgumentException("Invalid image format");

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_env.WebRootPath, ImageFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/{ImageFolder}/{fileName}";
        }

        private void EnsureImageDirectoryExists()
        {
            var path = Path.Combine(_env.WebRootPath, ImageFolder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
