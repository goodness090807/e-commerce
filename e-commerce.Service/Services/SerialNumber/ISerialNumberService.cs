using e_commerce.Data.Enums;

namespace e_commerce.Service.Services.SerialNumber
{
    public interface ISerialNumberService : IBaseService
    {
        Task<string> GenerateSerialNumberAsync(SerialNumberType serialNumberType);
    }
}
