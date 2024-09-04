using Microsoft.EntityFrameworkCore;

namespace e_commerce.Data.Models.Product
{
    public class ProductConfig : IEntityTypeConfiguration<ProductModel>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ProductModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.SKU).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(10000);
            builder.Property(x => x.Price).IsRequired();

            builder.Property(x => x.Done).HasColumnType("BIT");
            builder.Property(x => x.Launched).HasColumnType("BIT");
            // Price欄位小數點後兩位
            builder.Property(x => x.Price).HasPrecision(18, 2);

            // SKU 設定為唯一索引
            builder.HasIndex(x => x.SKU).IsUnique();
        }
    }
}
