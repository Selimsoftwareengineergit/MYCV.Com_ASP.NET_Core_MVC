using MYCV.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MYCV.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly string _rootFolder;

        public FileService(string rootFolder)
        {
            if (string.IsNullOrWhiteSpace(rootFolder))
                throw new ArgumentNullException(nameof(rootFolder));

            _rootFolder = rootFolder;
        }

        public async Task<string> UploadProfilePictureAsync(
            IFormFile file,
            int userId,
            bool isNewUser,
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

            // Final physical folder:
            // C:\MYCV\PersonalImage\<UserId>
            var userFolder = Path.Combine(_rootFolder, userId.ToString());

            // If new user → delete full folder
            if (isNewUser && Directory.Exists(userFolder))
            {
                Directory.Delete(userFolder, true);
            }

            if (!Directory.Exists(userFolder))
                Directory.CreateDirectory(userFolder);

            // If updating existing → delete only files
            if (!isNewUser && Directory.Exists(userFolder))
            {
                foreach (var existingFile in Directory.GetFiles(userFolder))
                {
                    File.Delete(existingFile);
                }
            }

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(userFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            // IMPORTANT:
            // Return relative path (store in DB)
            return $"{userId}/{fileName}";
        }
    }
}