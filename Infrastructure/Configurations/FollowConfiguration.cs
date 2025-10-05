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
    public class FollowConfiguration : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            builder.ToTable("follow");
            builder.HasKey(f => new { f.ShopId, f.UserProfileId });

            builder.Property(f => f.ShopId).HasColumnName("shop_id").HasMaxLength(36).IsRequired();
            builder.Property(f => f.UserProfileId).HasColumnName("user_profile_id").HasMaxLength(36).IsRequired(); // Reference only
            builder.Property(f => f.CreatedDate).HasColumnName("created_date").IsRequired();

            builder.HasIndex(f => f.UserProfileId);
        }
    }
}
