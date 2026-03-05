using MYCV.Application.Interfaces;
using Microsoft.AspNetCore.Http;

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

            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();

            var allowed = allowedExtensions
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.Trim())
                .ToArray();

            if (!allowed.Contains(extension))
                throw new ArgumentException($"Invalid file type. Allowed: {allowedExtensions}");

            if (file.Length > maxFileSizeMB * 1024 * 1024)
                throw new ArgumentException($"File size cannot exceed {maxFileSizeMB} MB.");

            // Physical folder: C:\MYCV\PersonalImage\<UserId>
            var userFolder = Path.Combine(_rootFolder, userId.ToString());

            try
            {
                // If new user → delete full folder
                if (isNewUser && Directory.Exists(userFolder))
                {
                    Directory.Delete(userFolder, true);
                }

                // Ensure folder exists
                Directory.CreateDirectory(userFolder);

                // If updating existing → delete only files
                if (!isNewUser)
                {
                    foreach (var existingFile in Directory.GetFiles(userFolder))
                    {
                        File.Delete(existingFile);
                    }
                }

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(userFolder, fileName);

                // Use a separate stream to avoid disposing issues
                await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
                await file.CopyToAsync(fileStream);

                // Return relative path for DB
                return $"{userId}/{fileName}";
            }
            catch (Exception ex)
            {
                // Log or handle exceptions if needed
                throw new IOException("Failed to upload profile picture.", ex);
            }
        }
    }
}