using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Services;
using Application.DTOs.Cart;
namespace KhoaLuanTotNghiepAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Lấy giỏ hàng của người dùng hiện tại
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMyCart(CancellationToken cancellationToken)
        {
            var result = await _cartService.GetMyCartAsync(cancellationToken);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Lấy số lượng sản phẩm trong giỏ hàng
        /// </summary>
        [HttpGet("count")]
        public async Task<IActionResult> GetCartItemCount(CancellationToken cancellationToken)
        {
            var result = await _cartService.GetCartItemCountAsync(cancellationToken);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Thêm sản phẩm vào giỏ hàng
        /// </summary>
        [HttpPost("items")]
        public async Task<IActionResult> AddToCart(
            [FromBody] AddToCartRequest request, CancellationToken cancellationToken)
        {
            var result = await _cartService.AddToCartAsync(request, cancellationToken);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Cập nhật số lượng sản phẩm trong giỏ hàng
        /// </summary>
        [HttpPut("items/{skuId}")]
        public async Task<IActionResult> UpdateCartItem(
            string skuId, [FromBody] UpdateCartItemRequest request, CancellationToken cancellationToken)
        {
            var result = await _cartService.UpdateCartItemAsync(skuId, request, cancellationToken);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Xóa sản phẩm khỏi giỏ hàng
        /// </summary>
        [HttpDelete("items/{skuId}")]
        public async Task<IActionResult> RemoveCartItem(string skuId, CancellationToken cancellationToken)
        {
            var result = await _cartService.RemoveCartItemAsync(skuId, cancellationToken);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Xóa toàn bộ giỏ hàng
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> ClearCart(CancellationToken cancellationToken)
        {
            var result = await _cartService.ClearCartAsync(cancellationToken);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Cập nhật trạng thái chọn sản phẩm
        /// </summary>
        [HttpPut("items/{skuId}/selection")]
        public async Task<IActionResult> UpdateItemSelection(
            string skuId, [FromBody] UpdateCartItemSelectionRequest request, CancellationToken cancellationToken)
        {
            var result = await _cartService.UpdateItemSelectionAsync(skuId, request, cancellationToken);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Cập nhật trạng thái chọn nhiều sản phẩm
        /// </summary>
        [HttpPut("items/batch-selection")]
        public async Task<IActionResult> BatchUpdateSelection(
            [FromBody] BatchUpdateSelectionRequest request, CancellationToken cancellationToken)
        {
            var result = await _cartService.BatchUpdateSelectionAsync(request, cancellationToken);
            return StatusCode(result.Code, result);
        }
    }
}
