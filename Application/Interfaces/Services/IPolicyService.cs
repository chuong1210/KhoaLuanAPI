using Application.DTOs.Policy;
using Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IPolicyService
    {
        Task<Result<PolicyDto>> GetPolicyByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<Result<PolicyDto>> GetActivePolicyByTypeAsync(string policyType, string shopId = null, CancellationToken cancellationToken = default);
        Task<Result<List<PolicySummaryDto>>> GetPolicyHistoryAsync(string policyType, string shopId = null, CancellationToken cancellationToken = default);
        Task<PaginatedResult<List<PolicyDto>>> GetPoliciesAsync(GetPoliciesQuery query, CancellationToken cancellationToken = default);
        Task<Result<PolicyDto>> CreatePolicyAsync(CreatePolicyRequest request, CancellationToken cancellationToken = default);
        Task<Result<PolicyDto>> UpdatePolicyAsync(string id, UpdatePolicyRequest request, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeletePolicyAsync(string id, CancellationToken cancellationToken = default);
        Task<Result<PolicyDto>> PublishPolicyAsync(string id, PublishPolicyRequest request, CancellationToken cancellationToken = default);
        Task<Result<Dictionary<string, PolicySummaryDto>>> GetAllActivePoliciesAsync(string shopId = null, CancellationToken cancellationToken = default);
    }
}
