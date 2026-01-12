using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Infrastructure.Services
{
    public class EmailTemplateService
    {
        private readonly string _rootPath;

        public EmailTemplateService(string rootPath)
        {
            _rootPath = rootPath;
        }

        public async Task<string> LoadTemplateAsync(string templateName)
        {
            var path = Path.Combine(
                _rootPath,
                "EmailTemplates",
                templateName);

            return await File.ReadAllTextAsync(path);
        }
    }
}