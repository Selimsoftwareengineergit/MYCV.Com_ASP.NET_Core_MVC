using Microsoft.AspNetCore.Http;
using MYCV.Application.Interfaces;

namespace MYCV.Infrastructure.Services
{
    public class FileService : IFileService
    {
        public async Task<string> UploadProfilePictureAsync(
            IFormFile file,
            int userId,
            string allowedExtensions = ".jpg,.jpeg,.png",
            int maxFileSizeMB = 2)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowed = allowedExtensions
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.Trim())
                .ToArray();

            if (!allowed.Contains(extension))
                throw new ArgumentException($"Invalid file type. Allowed: {allowedExtensions}");

            if (file.Length > maxFileSizeMB * 1024 * 1024)
                throw new ArgumentException($"File size cannot exceed {maxFileSizeMB} MB.");

            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "profile-pictures",
                userId.ToString());

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/profile-pictures/{userId}/{fileName}";
        }
    }
}