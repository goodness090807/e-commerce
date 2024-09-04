using e_commerce.Common.Models;
using e_commerce.Data;
using e_commerce.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace e_commerce.Service.Services.SerialNumber
{
    public class SerialNumberService : ISerialNumberService
    {
        private readonly ApplicationDbContext _context;

        public SerialNumberService(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<string> GenerateSerialNumberAsync(SerialNumberType serialNumberType)
        {
            // 重試策略 Jitter(參考網址：https://learn.microsoft.com/zh-tw/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly#add-a-jitter-strategy-to-the-retry-policy) 
            var jitterer = new Random();
            var retryPolicy = Policy.Handle<DbUpdateConcurrencyException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    + TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)));

            var serialNumberString = await retryPolicy.ExecuteAsync(async () =>
            {
                var serialNumber = await _context.SerialNumbers.FirstOrDefaultAsync(x => x.Type == serialNumberType);
                if (serialNumber == null)
                {
                    throw new BadRequestException("沒有這個類別");
                }

                if (serialNumber.LastGeneratedDate == DateTime.Today)
                {
                    serialNumber.CurrentNumber++;
                }
                else
                {
                    serialNumber.CurrentNumber = 1;
                    serialNumber.LastGeneratedDate = DateTime.Today;
                }

                // 製作樂觀鎖
                serialNumber.RowVersion = Guid.NewGuid();

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    await _context.Entry(serialNumber).ReloadAsync();
                    throw;
                }

                return serialNumber.Prefix + serialNumber.CurrentNumber.ToString().PadLeft(serialNumber.Length, '0');
            });

            return serialNumberString;
        }
    }
}
