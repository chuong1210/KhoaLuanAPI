using Application.Interfaces.Identity;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Infrastructure
{
    public class ShopDbContext: DbContext
    {
        private readonly EntitySaveChangesInterceptor? _saveChangesInterceptor;
        public ShopDbContext(DbContextOptions options    ,    EntitySaveChangesInterceptor? saveChangesInterceptor=null)

            : base(options)
        {
            _saveChangesInterceptor = saveChangesInterceptor;
        }
        // User & Auth
        public DbSet<Shop> Shops { get; set; }
        public DbSet<WalletShop> WalletShops { get; set; }
        public DbSet<ShopTransaction> ShopTransactions { get; set; }
        public DbSet<VoucherShop> VoucherShops { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Policy> Policies { get; set; }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ProgressTransfer> ProgressTransfers { get; set; }
        public DbSet<ClientTransfer> ClientTransfers { get; set; }
        public DbSet<ProgressClient> ProgressClients { get; set; }
        public DbSet<OfflineTransaction> OfflineTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShopDbContext).Assembly);
        }


        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
        //    {
        //        if (entry.State == EntityState.Added)
        //        {
        //            entry.Entity.CreatedBy = _currentUserService.UserId;
        //            entry.Entity.CreatedDate = DateTime.UtcNow;
        //        }

        //        if (entry.State == EntityState.Modified)
        //        {
        //            entry.Entity.ModifiedBy = _currentUserService.UserId;
        //            entry.Entity.ModifiedDate = DateTime.UtcNow;
        //        }
        //    }

        //    return await base.SaveChangesAsync(cancellationToken);
        //}
    }
}
