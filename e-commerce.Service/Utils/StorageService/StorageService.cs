using e_commerce.Common.Models;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;

namespace e_commerce.Service.Utils.StorageService
{
    public class StorageService : IStorageService
    {
        private readonly StorageClient _storageClient;
        private readonly AppSettings _appSettings;

        public StorageService(IOptions<AppSettings> options)
        {
            _storageClient = StorageClient.Create();
            _appSettings = options.Value;
        }

        public async Task UploadFileAsync(string pathWithFileName, string mimeType, Stream stream)
        {
            await _storageClient.UploadObjectAsync(_appSettings.GoogleStorage.BucketName, pathWithFileName, mimeType, stream);
        }

        public Task<bool> DeleteFileAsync(string filePath)
        {
            throw new NotImplementedException();
        }

    }
}
