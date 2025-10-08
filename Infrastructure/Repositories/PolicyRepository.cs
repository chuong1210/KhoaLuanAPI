using Domain.Entities;
using Domain.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{

    public class PolicyRepository : IPolicyRepository
    {
        private readonly ShopDbContext _context;
        private readonly ILogger<PolicyRepository> _logger;

        public PolicyRepository(ShopDbContext context, ILogger<PolicyRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Policy> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _context.Policies.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Policy> GetActiveByTypeAsync(string policyType, string shopId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Policies
                .Where(p => p.PolicyType == policyType && p.IsActive);

            if (!string.IsNullOrEmpty(shopId))
            {
                // Get shop-specific policy first, fallback to system policy
                var shopPolicy = await query
                    .Where(p => p.ShopId == shopId)
                    .OrderByDescending(p => p.Version)
                    .FirstOrDefaultAsync(cancellationToken);

                if (shopPolicy != null)
                    return shopPolicy;
            }

            // Get system policy
            return await query
                .Where(p => p.ShopId == null)
                .OrderByDescending(p => p.Version)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Policy>> GetAllByTypeAsync(string policyType, string shopId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Policies
                .Where(p => p.PolicyType == policyType);

            if (!string.IsNullOrEmpty(shopId))
            {
                query = query.Where(p => p.ShopId == shopId || p.ShopId == null);
            }
            else
            {
                query = query.Where(p => p.ShopId == null);
            }

            return await query
                .OrderByDescending(p => p.Version)
                .ToListAsync(cancellationToken);
        }

        public async Task<(List<Policy> Items, int TotalCount)> GetPaginatedAsync(
            int pageNumber, int pageSize, string policyType, bool? isActive, string shopId,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Policies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(policyType))
            {
                query = query.Where(p => p.PolicyType == policyType);
            }

            if (isActive.HasValue)
            {
                query = query.Where(p => p.IsActive == isActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(shopId))
            {
                query = query.Where(p => p.ShopId == shopId || p.ShopId == null);
            }
            else
            {
                query = query.Where(p => p.ShopId == null); // System policies only
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(p => p.CreatedDate)
                .ThenByDescending(p => p.Version)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<Policy> CreateAsync(Policy policy, CancellationToken cancellationToken = default)
        {
            await _context.Policies.AddAsync(policy, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Created policy {PolicyId} type {PolicyType} version {Version}",
                policy.Id, policy.PolicyType, policy.Version);
            return policy;
        }

        public async Task<Policy> UpdateAsync(Policy policy, CancellationToken cancellationToken = default)
        {
            _context.Policies.Update(policy);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Updated policy {PolicyId}", policy.Id);
            return policy;
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var policy = await _context.Policies.FindAsync(new object[] { id }, cancellationToken);
            if (policy == null) return false;

            _context.Policies.Remove(policy);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Deleted policy {PolicyId}", id);
            return true;
        }

        public async Task<int> GetNextVersionAsync(string policyType, string shopId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Policies
                .Where(p => p.PolicyType == policyType);

            if (!string.IsNullOrEmpty(shopId))
            {
                query = query.Where(p => p.ShopId == shopId);
            }
            else
            {
                query = query.Where(p => p.ShopId == null);
            }

            var maxVersion = await query.MaxAsync(p => (int?)p.Version, cancellationToken);
            return (maxVersion ?? 0) + 1;
        }

        public async Task<bool> DeactivateOldVersionsAsync(string policyType, string currentId, string shopId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Policies
                .Where(p => p.PolicyType == policyType && p.Id != currentId && p.IsActive);

            if (!string.IsNullOrEmpty(shopId))
            {
                query = query.Where(p => p.ShopId == shopId);
            }
            else
            {
                query = query.Where(p => p.ShopId == null);
            }

            var oldPolicies = await query.ToListAsync(cancellationToken);

            foreach (var policy in oldPolicies)
            {
                policy.IsActive = false;
            }

            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Deactivated {Count} old policies for type {PolicyType}",
                oldPolicies.Count, policyType);
            return true;
        }
    }
}
