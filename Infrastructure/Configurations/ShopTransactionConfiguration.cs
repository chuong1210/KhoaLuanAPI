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
    public class ShopTransactionConfiguration : IEntityTypeConfiguration<ShopTransaction>
    {
        public void Configure(EntityTypeBuilder<ShopTransaction> builder)
        {
            builder.ToTable("shop_transaction");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).HasColumnName("id").HasMaxLength(36).IsRequired();
            builder.Property(t => t.Status).HasColumnName("status").HasMaxLength(20).IsRequired();
            builder.Property(t => t.Type).HasColumnName("type").IsRequired();
            builder.Property(t => t.Amount).HasColumnName("amount").IsRequired();
            builder.Property(t => t.Message).HasColumnName("message").HasColumnType("TEXT");
            builder.Property(t => t.CreatedDate).HasColumnName("created_date").IsRequired();
            builder.Property(t => t.WalletShopId).HasColumnName("wallet_shop_id").HasMaxLength(36).IsRequired();
            builder.Property(t => t.OrderShopId).HasColumnName("order_shop_id").HasMaxLength(36); // Reference only

            builder.HasIndex(t => t.WalletShopId);
            builder.HasIndex(t => t.OrderShopId);
            builder.HasIndex(t => t.CreatedDate);
        }
    }
    }
