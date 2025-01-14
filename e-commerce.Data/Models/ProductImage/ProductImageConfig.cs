using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace e_commerce.Data.Models.ProductImage
{
    public class ProductImageConfig : IEntityTypeConfiguration<ProductImageModel>
    {
        public void Configure(EntityTypeBuilder<ProductImageModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ImageUrl).IsRequired().HasMaxLength(500);
            builder.Property(x => x.ImageTitle).IsRequired().HasMaxLength(100);
            builder.Property(x => x.IsMain).IsRequired().HasColumnType("BIT");
            builder.Property(x => x.Sort).IsRequired();

            builder.HasIndex(x => x.ImageUrl).IsUnique();
        }
    }
}
