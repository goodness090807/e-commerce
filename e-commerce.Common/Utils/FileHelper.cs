using Microsoft.AspNetCore.StaticFiles;

namespace e_commerce.Common.Utils
{
    public class FileHelper
    {
        public static (string extension, string mimeType) GetFileInfo(string uploadFileName)
        {
            var extension = Path.GetExtension(uploadFileName);
            var mimeType = GetMimeType(uploadFileName);

            return (extension, mimeType);
        }

        public static string GetMimeType(string fileName)
        {
            var extension = Path.GetExtension(fileName);

            if (extension != null && new FileExtensionContentTypeProvider().TryGetContentType(extension, out var mimeType))
            {
                return mimeType;
            }

            return "application/octet-stream";
        }
    }
}
