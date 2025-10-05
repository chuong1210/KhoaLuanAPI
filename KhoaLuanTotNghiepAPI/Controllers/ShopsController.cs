using Application.DTOs.Shop;
using Application.Interfaces.Services;
using Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KhoaLuanTotNghiepAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly IShopService _shopService;
        private readonly ILogger<ShopsController> _logger;

        public ShopsController(IShopService shopService, ILogger<ShopsController> logger)
        {
            _shopService = shopService;
            _logger = logger;
        }

        /// <summary>
        /// Get all shops with pagination and filters
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaginatedResult<List<ShopDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetShops([FromQuery] GetShopsQuery query, CancellationToken cancellationToken)
        {
            var result = await _shopService.GetShopsAsync(query, cancellationToken);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Get shop by ID
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Result<ShopDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetShopById(string id, CancellationToken cancellationToken)
        {
            var result = await _shopService.GetShopByIdAsync(id, cancellationToken);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Get current user's shop
        /// </summary>
        [HttpGet("my-shop")]
        [Authorize]
        [ProducesResponseType(typeof(Result<ShopDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMyShop(CancellationToken cancellationToken)
        {
            var result = await _shopService.GetMyShopAsync(cancellationToken);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Create new shop
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Result<ShopDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> CreateShop([FromBody] CreateShopRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating shop with name {ShopName}", request.ShopName);
            var result = await _shopService.CreateShopAsync(request, cancellationToken);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Update shop information
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Result<ShopDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateShop(string id, [FromBody] UpdateShopRequest request, CancellationToken cancellationToken)
        {
            var result = await _shopService.UpdateShopAsync(id, request, cancellationToken);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Delete shop
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Seller")]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteShop(string id, CancellationToken cancellationToken)
        {
            var result = await _shopService.DeleteShopAsync(id, cancellationToken);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Follow a shop
        /// </summary>
        [HttpPost("{shopId}/follow")]
        [Authorize]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> FollowShop(string shopId, CancellationToken cancellationToken)
        {
            var result = await _shopService.FollowShopAsync(shopId, cancellationToken);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Unfollow a shop
        /// </summary>
        [HttpDelete("{shopId}/follow")]
        [Authorize]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UnfollowShop(string shopId, CancellationToken cancellationToken)
        {
            var result = await _shopService.UnfollowShopAsync(shopId, cancellationToken);
            return StatusCode(result.Code, result);
        }
   
}
}

