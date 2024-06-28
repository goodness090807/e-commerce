using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace e_commerce.data.Models.RefreshToken
{
    public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshTokenModel>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.RefreshToken).IsRequired();
            builder.Property(x => x.ExpiredAt).IsRequired();

            builder.HasIndex(x => x.RefreshToken).IsUnique();

            // 關聯 User
            builder.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}
