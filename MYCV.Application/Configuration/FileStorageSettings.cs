using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Application.Configuration
{
    public class FileStorageSettings
    {
        /// <summary>
        /// Root folder for user profile images
        /// Example: "C:\MYCV\ProfileImages"
        /// </summary>
        public string ProfileImagesPath { get; set; } = string.Empty;
    }
}