using Application.DTOs.Policy;
using Application.Interfaces.Services;
using Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KhoaLuanTotNghiepAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PoliciesController : ControllerBase
    {
        private readonly IPolicyService _policyService;
        private readonly ILogger<PoliciesController> _logger;

        public PoliciesController(IPolicyService policyService, ILogger<PoliciesController> logger)
        {
            _policyService = policyService;
            _logger = logger;
        }

        /// <summary>
        /// Get active policy by type (public endpoint)
        /// </summary>
        [HttpGet("active/{policyType}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Result<PolicyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActivePolicyByType(
            string policyType,
            [FromQuery] string shopId,
            CancellationToken cancellationToken)
        {
            var result = await _policyService.GetActivePolicyByTypeAsync(policyType, shopId, cancellationToken);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Get all active policies
        /// </summary>
        [HttpGet("active")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Result<Dictionary<string, PolicySummaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllActivePolicies(
            [FromQuery] string shopId,
            CancellationToken cancellationToken)
        {
            var result = await _policyService.GetAllActivePoliciesAsync(shopId, cancellationToken);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Get policy version history
        /// </summary>
        [HttpGet("history/{policyType}")]
        [Authorize(Roles = "Admin,Seller")]
        [ProducesResponseType(typeof(Result<List<PolicySummaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPolicyHistory(
            string policyType,
            [FromQuery] string shopId,
            CancellationToken cancellationToken)
        {
            var result = await _policyService.GetPolicyHistoryAsync(policyType, shopId, cancellationToken);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Get all policies with pagination (admin/seller)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,Seller")]
        [ProducesResponseType(typeof(PaginatedResult<List<PolicyDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPolicies(
            [FromQuery] GetPoliciesQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _policyService.GetPoliciesAsync(query, cancellationToken);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Get policy by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Seller")]
        [ProducesResponseType(typeof(Result<PolicyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPolicyById(string id, CancellationToken cancellationToken)
        {
            var result = await _policyService.GetPolicyByIdAsync(id, cancellationToken);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Create new policy (draft)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Seller")]
        [ProducesResponseType(typeof(Result<PolicyDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreatePolicy(
            [FromBody] CreatePolicyRequest request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating policy type {PolicyType}", request.PolicyType);
            var result = await _policyService.CreatePolicyAsync(request, cancellationToken);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Update policy (only draft policies)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Seller")]
        [ProducesResponseType(typeof(Result<PolicyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePolicy(
string id,
[FromBody] UpdatePolicyRequest request,
CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating policy {PolicyId}", id);
            var result = await _policyService.UpdatePolicyAsync(id, request, cancellationToken);
            return StatusCode(result.Code, result);
        }
        /// 
        /// Delete policy (only draft policies)
        /// 
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Seller")]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePolicy(
        string id,
        CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting policy {PolicyId}", id);
            var result = await _policyService.DeletePolicyAsync(id, cancellationToken);
            return StatusCode(result.Code, result);
        }
        /// 
        /// Publish policy (activate and set effective date)
        /// 
        [HttpPost("{id}/publish")]
        [Authorize(Roles = "Admin,Seller")]
        [ProducesResponseType(typeof(Result<PolicyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PublishPolicy(
        string id,
        [FromBody] PublishPolicyRequest request,
        CancellationToken cancellationToken)
        {
            _logger.LogInformation("Publishing policy {PolicyId}", id);
            var result = await _policyService.PublishPolicyAsync(id, request, cancellationToken);
            return StatusCode(result.Code, result);
        }
    }
}
