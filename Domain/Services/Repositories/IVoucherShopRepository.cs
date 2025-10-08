using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Repositories
{
    public interface IVoucherShopRepository
    {
        Task<VoucherShop> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<VoucherShop> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
        Task<List<VoucherShop>> GetByShopIdAsync(string shopId, CancellationToken cancellationToken = default);
        Task<List<VoucherShop>> GetActiveByShopIdAsync(string shopId, CancellationToken cancellationToken = default);
        Task<(List<VoucherShop> Items, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize, string shopId, bool? isActive, CancellationToken cancellationToken = default);
        Task<VoucherShop> CreateAsync(VoucherShop voucher, CancellationToken cancellationToken = default);
        Task<VoucherShop> UpdateAsync(VoucherShop voucher, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> IncrementUsedAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> DecrementUsedAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> CanUseVoucherAsync(string voucherId, double orderAmount, CancellationToken cancellationToken = default);
    }
}
