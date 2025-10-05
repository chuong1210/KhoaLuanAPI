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
    public class ClientTransferConfiguration : IEntityTypeConfiguration<ClientTransfer>
    {
        public void Configure(EntityTypeBuilder<ClientTransfer> builder)
        {
            builder.ToTable("client_transfer");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("id").HasMaxLength(36).IsRequired();
            builder.Property(c => c.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
            builder.Property(c => c.AddressId).HasColumnName("address_id").HasMaxLength(36); // Reference only
            builder.Property(c => c.LocationGoogle).HasColumnName("location_google").HasMaxLength(500);

            // Cached fields from Profile Service
            builder.Property(c => c.CachedAddressLine).HasColumnName("cached_address_line").HasMaxLength(500);
            builder.Property(c => c.CachedCity).HasColumnName("cached_city").HasMaxLength(100);
            builder.Property(c => c.CachedPhone).HasColumnName("cached_phone").HasMaxLength(20);

            builder.HasIndex(c => c.AddressId);
        }
    }
}
