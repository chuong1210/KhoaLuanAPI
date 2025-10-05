using Application.Cache.Interfaces;
using Application.DTOs.Banner;
using Application.Interfaces.Identity;
using Application.Interfaces.Services;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using Domain.Services.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly ICacheService _cache;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ILogger<BannerService> _logger;

        public BannerService(
            IBannerRepository bannerRepository,
            ICacheService cache,
            ICurrentUserService currentUserService,
            IMapper mapper,
            ILogger<BannerService> logger)
        {
            _bannerRepository = bannerRepository;
            _cache = cache;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<BannerDto>> GetBannerByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var banner = await _bannerRepository.GetByIdAsync(id, cancellationToken);
            if (banner == null)
                return Result<BannerDto>.Failure("Banner không tồn tại", 404);

            var bannerDto = _mapper.Map<BannerDto>(banner);
            return Result<BannerDto>.Success(bannerDto, 200);
        }

        public async Task<Result<List<BannerDto>>> GetActiveBannersAsync(
            string bannerType = null, CancellationToken cancellationToken = default)
        {
            // Try cache first
            var cacheKey = $"banners:active:{bannerType ?? "all"}";
            var cached = await _cache.GetAsync<List<BannerDto>>(cacheKey);
            if (cached != null)
            {
                return Result<List<BannerDto>>.Success(cached, 200);
            }

            var banners = await _bannerRepository.GetActiveAsync(bannerType, cancellationToken);
            var bannerDtos = _mapper.Map<List<BannerDto>>(banners);

            // Cache for 10 minutes
            await _cache.SetAsync(cacheKey, bannerDtos, TimeSpan.FromMinutes(10));

            return Result<List<BannerDto>>.Success(bannerDtos, 200);
        }

        public async Task<PaginatedResult<List<BannerDto>>> GetBannersAsync(
            GetBannersQuery query, CancellationToken cancellationToken = default)
        {
            var (items, totalCount) = await _bannerRepository.GetPaginatedAsync(
                query.PageNumber, query.PageSize, query.BannerType, query.IsActive, cancellationToken);

            var bannerDtos = _mapper.Map<List<BannerDto>>(items);

            return PaginatedResult<List<BannerDto>>.Success(
                bannerDtos, totalCount, query.PageNumber, query.PageSize);
        }

        public async Task<Result<BannerDto>> CreateBannerAsync(
            CreateBannerRequest request, CancellationToken cancellationToken = default)
        {
            if (!_currentUserService.IsInRole("Admin"))
                return Result<BannerDto>.Forbidden();

            var banner = _mapper.Map<Banner>(request);
            var createdBanner = await _bannerRepository.CreateAsync(banner, cancellationToken);

            // Invalidate cache
            await _cache.RemoveAsync($"banners:active:{request.BannerType}");
            await _cache.RemoveAsync("banners:active:all");

            var bannerDto = _mapper.Map<BannerDto>(createdBanner);
            return Result<BannerDto>.Success(bannerDto, 201);
        }

        public async Task<Result<BannerDto>> UpdateBannerAsync(
            string id, UpdateBannerRequest request, CancellationToken cancellationToken = default)
        {
            if (!_currentUserService.IsInRole("Admin"))
                return Result<BannerDto>.Forbidden();

            var banner = await _bannerRepository.GetByIdAsync(id, cancellationToken);
            if (banner == null)
                return Result<BannerDto>.Failure("Banner không tồn tại", 404);

            _mapper.Map(request, banner);
            var updatedBanner = await _bannerRepository.UpdateAsync(banner, cancellationToken);

            // Invalidate all banner caches
            await _cache.RemoveAsync($"banners:active:{banner.BannerType}");
            await _cache.RemoveAsync("banners:active:all");

            var bannerDto = _mapper.Map<BannerDto>(updatedBanner);
            return Result<BannerDto>.Success(bannerDto, 200);
        }

        public async Task<Result<bool>> DeleteBannerAsync(string id, CancellationToken cancellationToken = default)
        {
            if (!_currentUserService.IsInRole("Admin"))
                return Result<bool>.Forbidden();

            var banner = await _bannerRepository.GetByIdAsync(id, cancellationToken);
            if (banner == null)
                return Result<bool>.Failure("Banner không tồn tại", 404);

            var result = await _bannerRepository.DeleteAsync(id, cancellationToken);

            // Invalidate cache
            await _cache.RemoveAsync($"banners:active:{banner.BannerType}");
            await _cache.RemoveAsync("banners:active:all");

            return Result<bool>.Success(result, 200);
        }

        public async Task<Result<bool>> UpdateBannerOrderAsync(
            UpdateBannerOrderRequest request, CancellationToken cancellationToken = default)
        {
            if (!_currentUserService.IsInRole("Admin"))
                return Result<bool>.Forbidden();

            var orders = request.Orders.Select(o => (o.Id, o.Order)).ToList();
            await _bannerRepository.UpdateOrderAsync(orders, cancellationToken);

            // Invalidate all caches
            await _cache.RemoveAsync("banners:active:all");

            return Result<bool>.Success(true, 200);
        }
    }
}
