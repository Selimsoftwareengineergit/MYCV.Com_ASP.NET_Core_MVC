using Microsoft.AspNetCore.Http;
using MYCV.Application.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MYCV.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private const string BaseFolder = @"C:\MYCV\ProfilePictures";

        public async Task<string> UploadProfilePictureAsync(
            IFormFile file,
            int userId,
            string allowedExtensions = ".jpg,.jpeg,.png",
            int maxFileSizeMB = 2)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowed = allowedExtensions.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                           .Select(e => e.Trim())
                                           .ToArray();

            if (!allowed.Contains(extension))
                throw new ArgumentException($"Invalid file type. Allowed: {allowedExtensions}");

            if (file.Length > maxFileSizeMB * 1024 * 1024)
                throw new ArgumentException($"File size cannot exceed {maxFileSizeMB} MB.");

            // Create user folder: C:\MYCV\ProfilePictures\{UserId}\
            var userFolder = Path.Combine(BaseFolder, userId.ToString());
            if (!Directory.Exists(userFolder))
                Directory.CreateDirectory(userFolder);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(userFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            // Return local file path (you can also return a relative URL if needed)
            return filePath;
        }
    }
}