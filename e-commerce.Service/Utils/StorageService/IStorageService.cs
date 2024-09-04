namespace e_commerce.Service.Utils.StorageService
{
    public interface IStorageService : IBaseService
    {
        Task UploadFileAsync(string pathWithFileName, string mimeType, Stream stream);
        Task<bool> DeleteFileAsync(string filePath);
    }
}
