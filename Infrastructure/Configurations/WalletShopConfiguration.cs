using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations
{
    public class WalletShopConfiguration : IEntityTypeConfiguration<WalletShop>
    {
        public void Configure(EntityTypeBuilder<WalletShop> builder)
        {
            builder.ToTable("wallet_shop");
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Id).HasColumnName("id").HasMaxLength(36).IsRequired();
            builder.Property(w => w.Amount).HasColumnName("amount").HasDefaultValue(0).IsRequired();
            builder.Property(w => w.ShopId).HasColumnName("shop_id").HasMaxLength(36).IsRequired();

            builder.HasMany(w => w.Transactions)
                .WithOne(t => t.WalletShop)
                .HasForeignKey(t => t.WalletShopId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(w => w.ShopId).IsUnique();
        }
    }
    }
