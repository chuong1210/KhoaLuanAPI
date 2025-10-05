using Application.DTOs.Shop;
using Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{

    public interface IShopService
    {
        Task<Result<ShopDto>> GetShopByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<Result<ShopDto>> GetMyShopAsync(CancellationToken cancellationToken = default);
        Task<PaginatedResult<List<ShopDto>>> GetShopsAsync(GetShopsQuery query, CancellationToken cancellationToken = default);
        Task<Result<ShopDto>> CreateShopAsync(CreateShopRequest request, CancellationToken cancellationToken = default);
        Task<Result<ShopDto>> UpdateShopAsync(string id, UpdateShopRequest request, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteShopAsync(string id, CancellationToken cancellationToken = default);
        Task<Result<bool>> FollowShopAsync(string shopId, CancellationToken cancellationToken = default);
        Task<Result<bool>> UnfollowShopAsync(string shopId, CancellationToken cancellationToken = default);
    }


}
