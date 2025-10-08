using Application.Cache.Interfaces;
using Application.Constants;
using Application.DTOs.Policy;
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

    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _policyRepository;
        private readonly ICacheService _cache;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ILogger<PolicyService> _logger;

        public PolicyService(
            IPolicyRepository policyRepository,
            ICacheService cache,
            ICurrentUserService currentUserService,
            IMapper mapper,
            ILogger<PolicyService> logger)
        {
            _policyRepository = policyRepository;
            _cache = cache;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<PolicyDto>> GetPolicyByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var policy = await _policyRepository.GetByIdAsync(id, cancellationToken);
            if (policy == null)
                return Result<PolicyDto>.Failure("Chính sách không tồn tại", 404);

            var policyDto = _mapper.Map<PolicyDto>(policy);
            return Result<PolicyDto>.Success(policyDto, 200);
        }

        public async Task<Result<PolicyDto>> GetActivePolicyByTypeAsync(
            string policyType, string shopId = null, CancellationToken cancellationToken = default)
        {
            // Validate policy type
            if (!PolicyTypes.All.Contains(policyType))
                return Result<PolicyDto>.Failure("Loại chính sách không hợp lệ", 400);

            // Try cache first
            var cacheKey = $"policy:active:{policyType}:{shopId ?? "system"}";
            var cached = await _cache.GetAsync<PolicyDto>(cacheKey);
            if (cached != null)
            {
                _logger.LogDebug("Policy cache hit for type {PolicyType}", policyType);
                return Result<PolicyDto>.Success(cached, 200);
            }

            var policy = await _policyRepository.GetActiveByTypeAsync(policyType, shopId, cancellationToken);
            if (policy == null)
                return Result<PolicyDto>.Failure("Chính sách không tồn tại", 404);

            var policyDto = _mapper.Map<PolicyDto>(policy);

            // Cache for 1 hour
            await _cache.SetAsync(cacheKey, policyDto, TimeSpan.FromHours(1));

            return Result<PolicyDto>.Success(policyDto, 200);
        }

        public async Task<Result<List<PolicySummaryDto>>> GetPolicyHistoryAsync(
            string policyType, string shopId = null, CancellationToken cancellationToken = default)
        {
            var policies = await _policyRepository.GetAllByTypeAsync(policyType, shopId, cancellationToken);
            var policyDtos = _mapper.Map<List<PolicySummaryDto>>(policies);

            return Result<List<PolicySummaryDto>>.Success(policyDtos, 200);
        }

        public async Task<PaginatedResult<List<PolicyDto>>> GetPoliciesAsync(
            GetPoliciesQuery query, CancellationToken cancellationToken = default)
        {
            var (items, totalCount) = await _policyRepository.GetPaginatedAsync(
                query.PageNumber, query.PageSize, query.PolicyType, query.IsActive, query.ShopId, cancellationToken);

            var policyDtos = _mapper.Map<List<PolicyDto>>(items);

            return PaginatedResult<List<PolicyDto>>.Success(
                policyDtos, totalCount, query.PageNumber, query.PageSize);
        }

        public async Task<Result<PolicyDto>> CreatePolicyAsync(
            CreatePolicyRequest request, CancellationToken cancellationToken = default)
        {
            // Check permission
            if (!string.IsNullOrEmpty(request.ShopId))
            {
                // Shop owner creating shop policy
                if (!_currentUserService.IsInRole("Seller"))
                    return Result<PolicyDto>.Forbidden();
            }
            else
            {
                // Admin creating system policy
                if (!_currentUserService.IsInRole("Admin"))
                    return Result<PolicyDto>.Forbidden();
            }

            // Get next version
            var nextVersion = await _policyRepository.GetNextVersionAsync(request.PolicyType, request.ShopId, cancellationToken);

            var policy = _mapper.Map<Policy>(request);
            policy.Version = nextVersion;
            policy.IsActive = false; // Draft by default

            var createdPolicy = await _policyRepository.CreateAsync(policy, cancellationToken);
            var policyDto = _mapper.Map<PolicyDto>(createdPolicy);

            _logger.LogInformation("Created policy {PolicyId} type {PolicyType} version {Version}",
                createdPolicy.Id, createdPolicy.PolicyType, createdPolicy.Version);

            return Result<PolicyDto>.Success(policyDto, 201);
        }

        public async Task<Result<PolicyDto>> UpdatePolicyAsync(
            string id, UpdatePolicyRequest request, CancellationToken cancellationToken = default)
        {
            var policy = await _policyRepository.GetByIdAsync(id, cancellationToken);
            if (policy == null)
                return Result<PolicyDto>.Failure("Chính sách không tồn tại", 404);

            // Check permission
            if (!string.IsNullOrEmpty(policy.ShopId))
            {
                if (!_currentUserService.IsInRole("Seller"))
                    return Result<PolicyDto>.Forbidden();
            }
            else
            {
                if (!_currentUserService.IsInRole("Admin"))
                    return Result<PolicyDto>.Forbidden();
            }

            // Don't allow editing active policies
            if (policy.IsActive)
                return Result<PolicyDto>.Failure("Không thể chỉnh sửa chính sách đang hoạt động. Vui lòng tạo phiên bản mới.", 400);

            _mapper.Map(request, policy);
            var updatedPolicy = await _policyRepository.UpdateAsync(policy, cancellationToken);

            var policyDto = _mapper.Map<PolicyDto>(updatedPolicy);

            return Result<PolicyDto>.Success(policyDto, 200);
        }

        public async Task<Result<bool>> DeletePolicyAsync(string id, CancellationToken cancellationToken = default)
        {
            var policy = await _policyRepository.GetByIdAsync(id, cancellationToken);
            if (policy == null)
                return Result<bool>.Failure("Chính sách không tồn tại", 404);

            // Check permission
            if (!string.IsNullOrEmpty(policy.ShopId))
            {
                if (!_currentUserService.IsInRole("Seller"))
                    return Result<bool>.Forbidden();
            }
            else
            {
                if (!_currentUserService.IsInRole("Admin"))
                    return Result<bool>.Forbidden();
            }

            // Don't allow deleting active policies
            if (policy.IsActive)
                return Result<bool>.Failure("Không thể xóa chính sách đang hoạt động", 400);

            var result = await _policyRepository.DeleteAsync(id, cancellationToken);
            return Result<bool>.Success(result, 200);
        }

        public async Task<Result<PolicyDto>> PublishPolicyAsync(
            string id, PublishPolicyRequest request, CancellationToken cancellationToken = default)
        {
            var policy = await _policyRepository.GetByIdAsync(id, cancellationToken);
            if (policy == null)
                return Result<PolicyDto>.Failure("Chính sách không tồn tại", 404);

            // Check permission
            if (!string.IsNullOrEmpty(policy.ShopId))
            {
                if (!_currentUserService.IsInRole("Seller"))
                    return Result<PolicyDto>.Forbidden();
            }
            else
            {
                if (!_currentUserService.IsInRole("Admin"))
                    return Result<PolicyDto>.Forbidden();
            }

            if (policy.IsActive)
                return Result<PolicyDto>.Failure("Chính sách đã được xuất bản", 400);

            // Deactivate old versions
            await _policyRepository.DeactivateOldVersionsAsync(policy.PolicyType, policy.Id, policy.ShopId, cancellationToken);

            // Activate this version
            policy.IsActive = true;
            policy.EffectiveDate = request.EffectiveDate;
            var updatedPolicy = await _policyRepository.UpdateAsync(policy, cancellationToken);

            // Invalidate cache
            var cacheKey = $"policy:active:{policy.PolicyType}:{policy.ShopId ?? "system"}";
            await _cache.RemoveAsync(cacheKey);

            var policyDto = _mapper.Map<PolicyDto>(updatedPolicy);

            _logger.LogInformation("Published policy {PolicyId} effective {EffectiveDate}",
                policy.Id, request.EffectiveDate);

            return Result<PolicyDto>.Success(policyDto, 200);
        }

        public async Task<Result<Dictionary<string, PolicySummaryDto>>> GetAllActivePoliciesAsync(
            string shopId = null, CancellationToken cancellationToken = default)
        {
            var result = new Dictionary<string, PolicySummaryDto>();

            foreach (var policyType in PolicyTypes.All)
            {
                var policy = await _policyRepository.GetActiveByTypeAsync(policyType, shopId, cancellationToken);
                if (policy != null)
                {
                    result[policyType] = _mapper.Map<PolicySummaryDto>(policy);
                }
            }

            return Result<Dictionary<string, PolicySummaryDto>>.Success(result, 200);
        }
    }
}
