using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Repositories
{
    public interface IPolicyRepository
    {
        Task<Policy> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<Policy> GetActiveByTypeAsync(string policyType, string shopId = null, CancellationToken cancellationToken = default);
        Task<List<Policy>> GetAllByTypeAsync(string policyType, string shopId = null, CancellationToken cancellationToken = default);
        Task<(List<Policy> Items, int TotalCount)> GetPaginatedAsync(
            int pageNumber, int pageSize, string policyType, bool? isActive, string shopId,
            CancellationToken cancellationToken = default);
        Task<Policy> CreateAsync(Policy policy, CancellationToken cancellationToken = default);
        Task<Policy> UpdateAsync(Policy policy, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<int> GetNextVersionAsync(string policyType, string shopId = null, CancellationToken cancellationToken = default);
        Task<bool> DeactivateOldVersionsAsync(string policyType, string currentId, string shopId = null, CancellationToken cancellationToken = default);
    }
}
