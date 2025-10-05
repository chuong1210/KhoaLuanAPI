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
    public class ShopConfiguration : IEntityTypeConfiguration<Shop>
    {
        public void Configure(EntityTypeBuilder<Shop> builder)
        {
            builder.ToTable("shop");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id).HasColumnName("id").HasMaxLength(36).IsRequired();
            builder.Property(s => s.ShopName).HasColumnName("shop_name").HasMaxLength(255).IsRequired();
            builder.Property(s => s.ShopDescription).HasColumnName("shop_description").HasColumnType("TEXT");
            builder.Property(s => s.ShopLogo).HasColumnName("shop_logo").HasColumnType("TEXT");
            builder.Property(s => s.ShopBanner).HasColumnName("shop_banner").HasColumnType("TEXT");
            builder.Property(s => s.ShopEmail).HasColumnName("shop_email").HasMaxLength(255);
            builder.Property(s => s.ShopPhone).HasColumnName("shop_phone").HasMaxLength(20);
            builder.Property(s => s.ShopStatus).HasColumnName("shop_status").HasDefaultValue(true);

            // Reference IDs only - NO navigation properties to external services
            builder.Property(s => s.ShopUserProfileId).HasColumnName("shop_user_profile_id").HasMaxLength(36).IsRequired();
            builder.Property(s => s.ShopAddressId).HasColumnName("shop_address_id").HasMaxLength(36);

            // Audit fields
            builder.Property(s => s.CreatedDate).HasColumnName("created_date").IsRequired();
            builder.Property(s => s.CreatedBy).HasColumnName("created_by").HasMaxLength(36);
            builder.Property(s => s.ModifiedDate).HasColumnName("modified_date");
            builder.Property(s => s.ModifiedBy).HasColumnName("modified_by").HasMaxLength(36);

            // Relationships within Shop Service
            builder.HasOne(s => s.Wallet)
                .WithOne(w => w.Shop)
                .HasForeignKey<WalletShop>(w => w.ShopId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Vouchers)
                .WithOne(v => v.Shop)
                .HasForeignKey(v => v.ShopId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Followers)
                .WithOne(f => f.Shop)
                .HasForeignKey(f => f.ShopId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(s => s.ShopUserProfileId);
            builder.HasIndex(s => s.ShopEmail);
            builder.HasIndex(s => s.ShopStatus);
        }
    }
}

