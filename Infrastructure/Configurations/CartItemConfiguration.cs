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
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("cart_item");
            builder.HasKey(ci => new { ci.CartId, ci.SkuId });

            builder.Property(ci => ci.CartId).HasColumnName("cart_id").HasMaxLength(36).IsRequired();
            builder.Property(ci => ci.SkuId).HasColumnName("sku_id").HasMaxLength(36).IsRequired(); // Reference only - NO FK
            builder.Property(ci => ci.Quantity).HasColumnName("quantity").IsRequired();
            builder.Property(ci => ci.IsSelected).HasColumnName("is_selected").HasDefaultValue(true);
            builder.Property(ci => ci.AddedDate).HasColumnName("added_date").IsRequired();

            // Cached fields from Product Service
            builder.Property(ci => ci.CachedProductName).HasColumnName("cached_product_name").HasMaxLength(500);
            builder.Property(ci => ci.CachedProductImage).HasColumnName("cached_product_image").HasColumnType("TEXT");
            builder.Property(ci => ci.CachedPrice).HasColumnName("cached_price").IsRequired();
            builder.Property(ci => ci.CachedShopId).HasColumnName("cached_shop_id").HasMaxLength(36);
            builder.Property(ci => ci.CachedAt).HasColumnName("cached_at").IsRequired();

            // NO navigation property to Sku - it's in Product Service
            // builder.HasOne(ci => ci.Sku) ❌ REMOVED

            builder.HasIndex(ci => ci.SkuId);
            builder.HasIndex(ci => ci.CachedAt); // For finding stale cache
            builder.HasIndex(ci => ci.CachedShopId); // For grouping by shop
        }
    }
}
