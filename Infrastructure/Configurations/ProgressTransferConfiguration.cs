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
    public class ProgressTransferConfiguration : IEntityTypeConfiguration<ProgressTransfer>
    {
        public void Configure(EntityTypeBuilder<ProgressTransfer> builder)
        {
            builder.ToTable("progress_transfer");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).HasColumnName("id").HasMaxLength(36).IsRequired();
            builder.Property(p => p.OrderShopId).HasColumnName("order_shop_id").HasMaxLength(36).IsRequired(); // Reference only
            builder.Property(p => p.EstimateTime).HasColumnName("estimate_time").IsRequired();
            builder.Property(p => p.Status).HasColumnName("status").HasMaxLength(20).IsRequired();
            builder.Property(p => p.BeginAddress).HasColumnName("begin_address").HasMaxLength(500).IsRequired();
            builder.Property(p => p.EndAddress).HasColumnName("end_address").HasMaxLength(500).IsRequired();

            builder.HasMany(p => p.ProgressClients)
                .WithOne(pc => pc.ProgressTransfer)
                .HasForeignKey(pc => pc.ProgressTransferId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.OrderShopId).IsUnique();
            builder.HasIndex(p => p.EstimateTime);
        }
    }
}
