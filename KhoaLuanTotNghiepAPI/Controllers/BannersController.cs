using Application.DTOs.Banner;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KhoaLuanTotNghiepAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BannersController : ControllerBase
    {
        private readonly IBannerService _bannerService;

        public BannersController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActiveBanners([FromQuery] string bannerType, CancellationToken cancellationToken)
        {
            var result = await _bannerService.GetActiveBannersAsync(bannerType, cancellationToken);
            return StatusCode(result.Code, result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBanners([FromQuery] GetBannersQuery query, CancellationToken cancellationToken)
        {
            var result = await _bannerService.GetBannersAsync(query, cancellationToken);
            return StatusCode(result.Code, result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBannerById(string id, CancellationToken cancellationToken)
        {
            var result = await _bannerService.GetBannerByIdAsync(id, cancellationToken);
            return StatusCode(result.Code, result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateBanner([FromBody] CreateBannerRequest request, CancellationToken cancellationToken)
        {
            var result = await _bannerService.CreateBannerAsync(request, cancellationToken);
            return StatusCode(result.Code, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBanner(string id, [FromBody] UpdateBannerRequest request, CancellationToken cancellationToken)
        {
            var result = await _bannerService.UpdateBannerAsync(id, request, cancellationToken);
            return StatusCode(result.Code, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBanner(string id, CancellationToken cancellationToken)
        {
            var result = await _bannerService.DeleteBannerAsync(id, cancellationToken);
            return StatusCode(result.Code, result);
        }

        [HttpPut("order")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBannerOrder([FromBody] UpdateBannerOrderRequest request, CancellationToken cancellationToken)
        {
            var result = await _bannerService.UpdateBannerOrderAsync(request, cancellationToken);
            return StatusCode(result.Code, result);
        }
    
}
}
