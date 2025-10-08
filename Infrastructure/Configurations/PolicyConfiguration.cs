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
    public class PolicyConfiguration : IEntityTypeConfiguration<Policy>
    {
        public void Configure(EntityTypeBuilder<Policy> builder)
        {
            builder.ToTable("policy");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).HasColumnName("id").HasMaxLength(36).IsRequired();
            builder.Property(p => p.PolicyName).HasColumnName("policy_name").HasMaxLength(255).IsRequired();
            builder.Property(p => p.PolicyContent).HasColumnName("policy_content").HasColumnType("TEXT").IsRequired();
            builder.Property(p => p.PolicyType).HasColumnName("policy_type").HasMaxLength(50).IsRequired();
            builder.Property(p => p.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            builder.Property(p => p.Version).HasColumnName("version").HasDefaultValue(1);
            builder.Property(p => p.EffectiveDate).HasColumnName("effective_date");
            builder.Property(p => p.ShopId).HasColumnName("shop_id").HasMaxLength(36); // NULL for system policies

            builder.Property(p => p.CreatedDate).HasColumnName("created_date").IsRequired();
            builder.Property(p => p.CreatedBy).HasColumnName("created_by").HasMaxLength(36);
            builder.Property(p => p.ModifiedDate).HasColumnName("modified_date");
            builder.Property(p => p.ModifiedBy).HasColumnName("modified_by").HasMaxLength(36);

            builder.HasIndex(p => p.PolicyType);
            builder.HasIndex(p => p.ShopId);
            builder.HasIndex(p => new { p.PolicyType, p.IsActive, p.ShopId });
            builder.HasIndex(p => new { p.PolicyType, p.Version });
        }
    }

}
