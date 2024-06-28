using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace e_commerce.data.Models.User
{
    public class UserConfig : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.HashedPassword).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Name).IsRequired();

            builder.HasIndex(x => x.Email).IsUnique();

            builder.Property(x => x.HashedPassword).HasMaxLength(200);
            builder.Property(x => x.Email).HasMaxLength(100);
            builder.Property(x => x.Name).HasMaxLength(50);
        }
    }
}
