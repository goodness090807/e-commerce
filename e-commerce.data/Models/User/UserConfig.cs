using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.data.Models.User
{
    public class UserConfig : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Account).IsRequired();
            builder.Property(x => x.HashedPassword).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Name).IsRequired();

            builder.HasIndex(x => x.Account).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();

            // 每個欄位都加上MaxLength
            builder.Property(x => x.Account).HasMaxLength(50);
            builder.Property(x => x.HashedPassword).HasMaxLength(200);
            builder.Property(x => x.Email).HasMaxLength(100);
            builder.Property(x => x.Name).HasMaxLength(50);
        }
    }
}
