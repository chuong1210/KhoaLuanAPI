using Domain.Entities;
using Domain.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{

    public class BannerRepository : IBannerRepository
    {
        private readonly ShopDbContext _context;

        public BannerRepository(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<Banner> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _context.Banners.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<List<Banner>> GetActiveAsync(string bannerType = null, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var query = _context.Banners
                .Where(b => b.IsActive &&
                           (!b.StartDate.HasValue || b.StartDate <= now) &&
                           (!b.EndDate.HasValue || b.EndDate >= now));

            if (!string.IsNullOrWhiteSpace(bannerType))
            {
                query = query.Where(b => b.BannerType == bannerType);
            }

            return await query.OrderBy(b => b.BannerOrder).ToListAsync(cancellationToken);
        }

        public async Task<(List<Banner> Items, int TotalCount)> GetPaginatedAsync(
            int pageNumber, int pageSize, string bannerType, bool? isActive, CancellationToken cancellationToken = default)
        {
            var query = _context.Banners.AsQueryable();

            if (!string.IsNullOrWhiteSpace(bannerType))
            {
                query = query.Where(b => b.BannerType == bannerType);
            }

            if (isActive.HasValue)
            {
                query = query.Where(b => b.IsActive == isActive.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderBy(b => b.BannerOrder)
                .ThenByDescending(b => b.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<Banner> CreateAsync(Banner banner, CancellationToken cancellationToken = default)
        {
            await _context.Banners.AddAsync(banner, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return banner;
        }

        public async Task<Banner> UpdateAsync(Banner banner, CancellationToken cancellationToken = default)
        {
            _context.Banners.Update(banner);
            await _context.SaveChangesAsync(cancellationToken);
            return banner;
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var banner = await _context.Banners.FindAsync(new object[] { id }, cancellationToken);
            if (banner == null) return false;

            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> UpdateOrderAsync(List<(string Id, int Order)> orders, CancellationToken cancellationToken = default)
        {
            foreach (var (id, order) in orders)
            {
                var banner = await _context.Banners.FindAsync(new object[] { id }, cancellationToken);
                if (banner != null)
                {
                    banner.BannerOrder = order;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    
}
}
