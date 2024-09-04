using e_commerce.Common.Utils;
using e_commerce.Data.Models;
using e_commerce.Data.Models.Product;
using e_commerce.Data.Models.ProductSEO;
using e_commerce.Data.Models.RefreshToken;
using e_commerce.Data.Models.SerialNumber;
using e_commerce.Data.Models.User;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Data
{
    public  class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions optinos) : base(optinos)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<RefreshTokenModel> RefreshTokens { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ProductSEOModel> ProductSEOs { get; set; }
        public DbSet<SerialNumberModel> SerialNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // 配置此dll底下的所有config
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var entries = ChangeTracker
                .Entries<IAuditable>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Entity.CreatedAt = DateTimeHelper.GetUTC8Now();
                }

                entityEntry.Entity.UpdatedAt = DateTimeHelper.GetUTC8Now();
            }

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries<IAuditable>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Entity.CreatedAt = DateTimeHelper.GetUTC8Now();
                }

                entityEntry.Entity.UpdatedAt = DateTimeHelper.GetUTC8Now();
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
