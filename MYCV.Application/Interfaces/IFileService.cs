using Microsoft.AspNetCore.Http;

namespace MYCV.Application.Interfaces
{
    public interface IFileService
    {
        /// <summary>
        /// Uploads a profile picture and returns the public URL.
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <param name="userId">User ID folder</param>
        /// <param name="allowedExtensions">Allowed file extensions (e.g. .jpg,.png)</param>
        /// <param name="maxFileSizeMB">Maximum file size in MB</param>
        /// <returns>Public URL of the uploaded file</returns>
        Task<string> UploadProfilePictureAsync(
            IFormFile file,
            int userId,
            string allowedExtensions = ".jpg,.jpeg,.png",
            int maxFileSizeMB = 2);
    }
}
