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
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("cart");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("id").HasMaxLength(36).IsRequired();
            builder.Property(c => c.UserProfileId).HasColumnName("user_profile_id").HasMaxLength(36).IsRequired(); // Reference only
            builder.Property(c => c.CreatedDate).HasColumnName("created_date").IsRequired();
            builder.Property(c => c.UpdatedDate).HasColumnName("updated_date").IsRequired();

            builder.HasMany(c => c.Items)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(c => c.UserProfileId).IsUnique();
        }
    }

   
}
