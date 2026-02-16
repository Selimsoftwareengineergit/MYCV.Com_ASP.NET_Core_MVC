using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MYCV.Application.Interfaces
{
    public interface IFileService
    {
        /// <summary>
        /// Uploads a file and returns the relative URL
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <param name="userId">User folder ID</param>
        /// <param name="isNewUser">True if creating new record (delete folder), false if updating (replace existing file)</param>
        /// <param name="allowedExtensions">Allowed file extensions</param>
        /// <param name="maxFileSizeMB">Max file size in MB</param>
        /// <returns>Relative URL of uploaded file</returns>
        Task<string> UploadProfilePictureAsync(
            IFormFile file,
            int userId,
            bool isNewUser,
            string allowedExtensions = ".jpg,.jpeg,.png",
            int maxFileSizeMB = 2);
    }
}