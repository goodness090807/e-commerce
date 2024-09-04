using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace e_commerce.Data.Utils
{
    public static class EFCoreConverter
    {
        /// <summary>
        /// Enum 轉換成字串
        /// </summary>
        /// <typeparam name="TEnum">任意Enum</typeparam>
        public static ValueConverter<TEnum, string> EnumToStringConverter<TEnum>()
            where TEnum : struct
        {
            return new ValueConverter<TEnum, string>(
                v => v.ToString()!,
                v => (TEnum)Enum.Parse(typeof(TEnum), v));
        }
    }
}
