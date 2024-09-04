using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace e_commerce.Data.Models.ProductSEO
{
    public class ProductSEOConfig : IEntityTypeConfiguration<ProductSEOModel>
    {
        public void Configure(EntityTypeBuilder<ProductSEOModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.MetaTitle).HasMaxLength(100);
            builder.Property(x => x.MetaDescription).HasMaxLength(10000);
            builder.Property(x => x.MetaPictureUrl).HasMaxLength(200);

            builder.HasOne(x => x.Product)
                .WithOne(x => x.ProductSEO)
                .HasForeignKey<ProductSEOModel>(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
