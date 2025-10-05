using Domain.Entities;
using Domain.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{

    public class ShopRepository : IShopRepository
    {
        private readonly ShopDbContext _context;
        private readonly ILogger<ShopRepository> _logger;

        public ShopRepository(ShopDbContext context, ILogger<ShopRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Shop> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _context.Shops
                .Include(s => s.Wallet)
                .Include(s => s.Vouchers.Where(v => v.IsActive))
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<Shop> GetByUserProfileIdAsync(string userProfileId, CancellationToken cancellationToken = default)
        {
            return await _context.Shops
                .Include(s => s.Wallet)
                .FirstOrDefaultAsync(s => s.ShopUserProfileId == userProfileId, cancellationToken);
        }

        public async Task<(List<Shop> Items, int TotalCount)> GetPaginatedAsync(
            int pageNumber, int pageSize, string searchTerm, bool? status, CancellationToken cancellationToken = default)
        {
            var query = _context.Shops
                .Include(s => s.Wallet)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s =>
                    s.ShopName.Contains(searchTerm) ||
                    s.ShopDescription.Contains(searchTerm) ||
                    s.ShopEmail.Contains(searchTerm));
            }

            if (status.HasValue)
            {
                query = query.Where(s => s.ShopStatus == status.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(s => s.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<Shop> CreateAsync(Shop shop, CancellationToken cancellationToken = default)
        {
            await _context.Shops.AddAsync(shop, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return shop;
        }

        public async Task<Shop> UpdateAsync(Shop shop, CancellationToken cancellationToken = default)
        {
            _context.Shops.Update(shop);
            await _context.SaveChangesAsync(cancellationToken);
            return shop;
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var shop = await _context.Shops.FindAsync(new object[] { id }, cancellationToken);
            if (shop == null) return false;

            _context.Shops.Remove(shop);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _context.Shops.AnyAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<int> GetFollowerCountAsync(string shopId, CancellationToken cancellationToken = default)
        {
            return await _context.Follows
                .Where(f => f.ShopId == shopId)
                .CountAsync(cancellationToken);
        }

        public async Task<bool> IsFollowingAsync(string shopId, string userProfileId, CancellationToken cancellationToken = default)
        {
            return await _context.Follows
                .AnyAsync(f => f.ShopId == shopId && f.UserProfileId == userProfileId, cancellationToken);
        }

        public async Task<bool> FollowAsync(string shopId, string userProfileId, CancellationToken cancellationToken = default)
        {
            var exists = await IsFollowingAsync(shopId, userProfileId, cancellationToken);
            if (exists) return false;

            var follow = new Follow
            {
                ShopId = shopId,
                UserProfileId = userProfileId,
                CreatedDate = DateTime.UtcNow
            };

            await _context.Follows.AddAsync(follow, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> UnfollowAsync(string shopId, string userProfileId, CancellationToken cancellationToken = default)
        {
            var follow = await _context.Follows
                .FirstOrDefaultAsync(f => f.ShopId == shopId && f.UserProfileId == userProfileId, cancellationToken);

            if (follow == null) return false;

            _context.Follows.Remove(follow);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
