using Application.DTOs.Banner;
using Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IBannerService
    {

        Task<Result<BannerDto>> GetBannerByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<Result<List<BannerDto>>> GetActiveBannersAsync(string bannerType = null, CancellationToken cancellationToken = default);
        Task<PaginatedResult<List<BannerDto>>> GetBannersAsync(GetBannersQuery query, CancellationToken cancellationToken = default);
        Task<Result<BannerDto>> CreateBannerAsync(CreateBannerRequest request, CancellationToken cancellationToken = default);
        Task<Result<BannerDto>> UpdateBannerAsync(string id, UpdateBannerRequest request, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteBannerAsync(string id, CancellationToken cancellationToken = default);
        Task<Result<bool>> UpdateBannerOrderAsync(UpdateBannerOrderRequest request, CancellationToken cancellationToken = default);
    }

}
