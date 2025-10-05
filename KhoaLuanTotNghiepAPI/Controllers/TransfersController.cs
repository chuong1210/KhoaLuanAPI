using Application.DTOs.Transfer;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KhoaLuanTotNghiepAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransfersController : ControllerBase
    {
        private readonly ITransferService _transferService;

        public TransfersController(ITransferService transferService)
        {
            _transferService = transferService;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetProgressTransferById(string id, CancellationToken cancellationToken)
        {
            var result = await _transferService.GetProgressTransferByIdAsync(id, cancellationToken);
            return StatusCode(result.Code, result);
        }

        [HttpGet("order-shop/{orderShopId}")]
        [Authorize]
        public async Task<IActionResult> GetProgressTransferByOrderShopId(string orderShopId, CancellationToken cancellationToken)
        {
            var result = await _transferService.GetProgressTransferByOrderShopIdAsync(orderShopId, cancellationToken);
            return StatusCode(result.Code, result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Shipper")]
        public async Task<IActionResult> CreateProgressTransfer(
            [FromBody] CreateProgressTransferRequest request, CancellationToken cancellationToken)
        {
            var result = await _transferService.CreateProgressTransferAsync(request, cancellationToken);
            return StatusCode(result.Code, result);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,Shipper")]
        public async Task<IActionResult> UpdateProgressTransferStatus(
            string id, [FromBody] UpdateProgressTransferStatusRequest request, CancellationToken cancellationToken)
        {
            var result = await _transferService.UpdateProgressTransferStatusAsync(id, request, cancellationToken);
            return StatusCode(result.Code, result);
        }

        [HttpPost("offline-transaction")]
        [Authorize(Roles = "Shipper")]
        public async Task<IActionResult> CreateOfflineTransaction(
            [FromBody] CreateOfflineTransactionRequest request, CancellationToken cancellationToken)
        {
            var result = await _transferService.CreateOfflineTransactionAsync(request, cancellationToken);
            return StatusCode(result.Code, result);
        }
}
}

