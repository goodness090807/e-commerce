using e_commerce.Data.Enums;
using e_commerce.Data.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace e_commerce.Data.Models.SerialNumber
{
    public class SerialNumberConfig : IEntityTypeConfiguration<SerialNumberModel>
    {
        public void Configure(EntityTypeBuilder<SerialNumberModel> builder)
        {
            var converter = EFCoreConverter.EnumToStringConverter<SerialNumberType>();

            builder.Property(x => x.Type).IsRequired().HasConversion(converter);
            builder.Property(x => x.Prefix).HasMaxLength(10);
            builder.Property(x => x.Length).IsRequired();
            builder.Property(x => x.CurrentNumber).IsRequired();
            builder.Property(x => x.LastGeneratedDate).IsRequired().HasColumnType("DATETIME");
            builder.Property(x => x.RowVersion).IsConcurrencyToken();

            builder.HasIndex(x => x.Type).IsUnique();

            builder.HasData(
                new SerialNumberModel()
                {
                    Id = 1,
                    Type = SerialNumberType.SKU,
                    Prefix = "PD",
                    CurrentNumber = 1,
                    LastGeneratedDate = DateTime.MinValue,
                    Length = 10,
                    RowVersion = new Guid("cf1f568f-1a6b-44da-97d5-54ca3261eb30") // 這個GUID寫死是防止EFCore生成的數據不一致
                });
        }
    }
}
