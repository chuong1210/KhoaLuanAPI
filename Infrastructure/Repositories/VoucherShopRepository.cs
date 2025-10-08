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

    public class VoucherShopRepository : IVoucherShopRepository
    {
        private readonly ShopDbContext _context;
        private readonly ILogger<VoucherShopRepository> _logger;

        public VoucherShopRepository(ShopDbContext context, ILogger<VoucherShopRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<VoucherShop> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _context.VoucherShops
                .Include(v => v.Shop)
                .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        }

        public async Task<VoucherShop> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            return await _context.VoucherShops
                .Include(v => v.Shop)
                .FirstOrDefaultAsync(v => v.Code == code && v.IsActive, cancellationToken);
        }

        public async Task<List<VoucherShop>> GetByShopIdAsync(string shopId, CancellationToken cancellationToken = default)
        {
            return await _context.VoucherShops
                .Where(v => v.ShopId == shopId)
                .OrderByDescending(v => v.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<VoucherShop>> GetActiveByShopIdAsync(string shopId, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            return await _context.VoucherShops
                .Where(v => v.ShopId == shopId &&
                           v.IsActive &&
                           v.StartAvailable <= now &&
                           v.End >= now &&
                           v.Used < v.Quantity)
                .OrderByDescending(v => v.Discount)
                .ToListAsync(cancellationToken);
        }

        public async Task<(List<VoucherShop> Items, int TotalCount)> GetPaginatedAsync(
            int pageNumber, int pageSize, string shopId, bool? isActive, CancellationToken cancellationToken = default)
        {
            var query = _context.VoucherShops.AsQueryable();

            if (!string.IsNullOrEmpty(shopId))
            {
                query = query.Where(v => v.ShopId == shopId);
            }

            if (isActive.HasValue)
            {
                query = query.Where(v => v.IsActive == isActive.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(v => v.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<VoucherShop> CreateAsync(VoucherShop voucher, CancellationToken cancellationToken = default)
        {
            await _context.VoucherShops.AddAsync(voucher, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Created voucher {VoucherId} for shop {ShopId}", voucher.Id, voucher.ShopId);
            return voucher;
        }

        public async Task<VoucherShop> UpdateAsync(VoucherShop voucher, CancellationToken cancellationToken = default)
        {
            _context.VoucherShops.Update(voucher);
            await _context.SaveChangesAsync(cancellationToken);
            return voucher;
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var voucher = await _context.VoucherShops.FindAsync(new object[] { id }, cancellationToken);
            if (voucher == null) return false;

            _context.VoucherShops.Remove(voucher);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> IncrementUsedAsync(string id, CancellationToken cancellationToken = default)
        {
            var voucher = await _context.VoucherShops.FindAsync(new object[] { id }, cancellationToken);
            if (voucher == null) return false;

            voucher.Used++;
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Incremented usage for voucher {VoucherId}, now: {Used}/{Quantity}",
                id, voucher.Used, voucher.Quantity);
            return true;
        }

        public async Task<bool> DecrementUsedAsync(string id, CancellationToken cancellationToken = default)
        {
            var voucher = await _context.VoucherShops.FindAsync(new object[] { id }, cancellationToken);
            if (voucher == null || voucher.Used <= 0) return false;

            voucher.Used--;
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Decremented usage for voucher {VoucherId}, now: {Used}/{Quantity}",
                id, voucher.Used, voucher.Quantity);
            return true;
        }

        public async Task<bool> CanUseVoucherAsync(string voucherId, double orderAmount, CancellationToken cancellationToken = default)
        {
            var voucher = await GetByIdAsync(voucherId, cancellationToken);
            if (voucher == null) return false;

            var now = DateTime.UtcNow;

            return voucher.IsActive &&
                   voucher.StartAvailable <= now &&
                   voucher.End >= now &&
                   voucher.Used < voucher.Quantity &&
                   orderAmount >= voucher.MinSupport;
        }
    }
}
