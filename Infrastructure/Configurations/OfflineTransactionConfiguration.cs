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
    public class OfflineTransactionConfiguration : IEntityTypeConfiguration<OfflineTransaction>
    {
        public void Configure(EntityTypeBuilder<OfflineTransaction> builder)
        {
            builder.ToTable("offline_transaction");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id).HasColumnName("id").HasMaxLength(36).IsRequired();
            builder.Property(o => o.Amount).HasColumnName("amount").IsRequired();
            builder.Property(o => o.OrderShopId).HasColumnName("order_shop_id").HasMaxLength(36).IsRequired(); // Reference only
            builder.Property(o => o.CreatedDate).HasColumnName("created_date").IsRequired();
            builder.Property(o => o.Message).HasColumnName("message").HasColumnType("TEXT");

            builder.HasIndex(o => o.OrderShopId);
            builder.HasIndex(o => o.CreatedDate);
        }
    }
}
