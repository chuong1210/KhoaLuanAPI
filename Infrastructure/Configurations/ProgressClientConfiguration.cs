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
    public class ProgressClientConfiguration : IEntityTypeConfiguration<ProgressClient>
    {
        public void Configure(EntityTypeBuilder<ProgressClient> builder)
        {
            builder.ToTable("progress_client");
            builder.HasKey(pc => new { pc.ProgressTransferId, pc.ClientTransferId });

            builder.Property(pc => pc.Sort).HasColumnName("sort").IsRequired();
            builder.Property(pc => pc.TimeTo).HasColumnName("time_to").IsRequired();
            builder.Property(pc => pc.ProgressTransferId).HasColumnName("progress_transfer_id").HasMaxLength(36).IsRequired();
            builder.Property(pc => pc.ClientTransferId).HasColumnName("client_transfer_id").HasMaxLength(36).IsRequired();

            builder.HasOne(pc => pc.ClientTransfer)
                .WithMany()
                .HasForeignKey(pc => pc.ClientTransferId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(pc => pc.Sort);
        }
    }
}
