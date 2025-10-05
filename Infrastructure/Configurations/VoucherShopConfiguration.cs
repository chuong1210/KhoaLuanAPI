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
    public class VoucherShopConfiguration : IEntityTypeConfiguration<VoucherShop>
    {
        public void Configure(EntityTypeBuilder<VoucherShop> builder)
        {
            builder.ToTable("voucher_shop");
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Id).HasColumnName("id").HasMaxLength(36).IsRequired();
            builder.Property(v => v.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
            builder.Property(v => v.Discount).HasColumnName("discount").IsRequired();
            builder.Property(v => v.StartAvailable).HasColumnName("start_available").IsRequired();
            builder.Property(v => v.End).HasColumnName("end").IsRequired();
            builder.Property(v => v.MinSupport).HasColumnName("min_support").IsRequired();
            builder.Property(v => v.MaxDiscount).HasColumnName("max_discount").IsRequired();
            builder.Property(v => v.ShopId).HasColumnName("shop_id").HasMaxLength(36).IsRequired();
            builder.Property(v => v.Image).HasColumnName("image").HasColumnType("TEXT");
            builder.Property(v => v.CategoryId).HasColumnName("category_id").HasMaxLength(36); // Reference only
            builder.Property(v => v.Quantity).HasColumnName("quantity").HasDefaultValue(0);
            builder.Property(v => v.Used).HasColumnName("used").HasDefaultValue(0);
            builder.Property(v => v.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
            builder.Property(v => v.IsActive).HasColumnName("is_active").HasDefaultValue(true);

            builder.Property(v => v.CreatedDate).HasColumnName("created_date").IsRequired();
            builder.Property(v => v.CreatedBy).HasColumnName("created_by").HasMaxLength(36);
            builder.Property(v => v.ModifiedDate).HasColumnName("modified_date");
            builder.Property(v => v.ModifiedBy).HasColumnName("modified_by").HasMaxLength(36);

            builder.HasIndex(v => v.Code).IsUnique();
            builder.HasIndex(v => v.ShopId);
            builder.HasIndex(v => new { v.StartAvailable, v.End, v.IsActive });
        }
    }
}
