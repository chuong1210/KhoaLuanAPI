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
    public class BannerConfiguration : IEntityTypeConfiguration<Banner>
    {
        public void Configure(EntityTypeBuilder<Banner> builder)
        {

            builder.ToTable("banner");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id).HasColumnName("id").HasMaxLength(36).IsRequired();
            builder.Property(b => b.BannerName).HasColumnName("banner_name").HasMaxLength(255);
            builder.Property(b => b.BannerImage).HasColumnName("banner_image").HasMaxLength(500);
            builder.Property(b => b.BannerUrl).HasColumnName("banner_url").HasColumnType("TEXT");
            builder.Property(b => b.BannerOrder).HasColumnName("banner_order").HasDefaultValue(0);
            builder.Property(b => b.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            builder.Property(b => b.StartDate).HasColumnName("start_date");
            builder.Property(b => b.EndDate).HasColumnName("end_date");
            builder.Property(b => b.BannerType).HasColumnName("banner_type").HasMaxLength(50);
            builder.Property(b => b.TargetId).HasColumnName("target_id").HasMaxLength(36); // Reference only

            builder.Property(b => b.CreatedDate).HasColumnName("created_date").IsRequired();
            builder.Property(b => b.CreatedBy).HasColumnName("created_by").HasMaxLength(36);
            builder.Property(b => b.ModifiedDate).HasColumnName("modified_date");
            builder.Property(b => b.ModifiedBy).HasColumnName("modified_by").HasMaxLength(36);

            builder.HasIndex(b => new { b.BannerType, b.IsActive, b.BannerOrder });
            builder.HasIndex(b => new { b.StartDate, b.EndDate });
        }
    }
}
