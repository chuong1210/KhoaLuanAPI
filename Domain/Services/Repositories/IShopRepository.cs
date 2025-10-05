using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Repositories
{
    public interface IShopRepository
    {
        Task<Shop> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<Shop> GetByUserProfileIdAsync(string userProfileId, CancellationToken cancellationToken = default);
        Task<(List<Shop> Items, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize, string searchTerm, bool? status, CancellationToken cancellationToken = default);
        Task<Shop> CreateAsync(Shop shop, CancellationToken cancellationToken = default);
        Task<Shop> UpdateAsync(Shop shop, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default);
        Task<int> GetFollowerCountAsync(string shopId, CancellationToken cancellationToken = default);
        Task<bool> IsFollowingAsync(string shopId, string userProfileId, CancellationToken cancellationToken = default);
        Task<bool> FollowAsync(string shopId, string userProfileId, CancellationToken cancellationToken = default);
        Task<bool> UnfollowAsync(string shopId, string userProfileId, CancellationToken cancellationToken = default);
    }
}
