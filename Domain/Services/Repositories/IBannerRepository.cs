using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Repositories
{

    public interface IBannerRepository
    {
        Task<Banner> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<List<Banner>> GetActiveAsync(string bannerType = null, CancellationToken cancellationToken = default);
        Task<(List<Banner> Items, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize, string bannerType, bool? isActive, CancellationToken cancellationToken = default);
        Task<Banner> CreateAsync(Banner banner, CancellationToken cancellationToken = default);
        Task<Banner> UpdateAsync(Banner banner, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> UpdateOrderAsync(List<(string Id, int Order)> orders, CancellationToken cancellationToken = default);
    }
}
