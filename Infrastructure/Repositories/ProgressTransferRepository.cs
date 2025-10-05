using Domain.Entities;
using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Services.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repositories
{
    public class ProgressTransferRepository : IProgressTransferRepository
    {
        private readonly ShopDbContext _context;

        public ProgressTransferRepository(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<ProgressTransfer> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _context.ProgressTransfers
                .Include(p => p.ProgressClients)
                    .ThenInclude(pc => pc.ClientTransfer)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<ProgressTransfer> GetByOrderShopIdAsync(string orderShopId, CancellationToken cancellationToken = default)
        {
            return await _context.ProgressTransfers
                .Include(p => p.ProgressClients)
                    .ThenInclude(pc => pc.ClientTransfer)
                .FirstOrDefaultAsync(p => p.OrderShopId == orderShopId, cancellationToken);
        }

        public async Task<List<ProgressTransfer>> GetByUserProfileIdAsync(string userProfileId, CancellationToken cancellationToken = default)
        {
            // Note: Cần join với Order Service để lấy orders của user
            // Trong microservices, cần gọi Order Service hoặc lưu UserProfileId trong ProgressTransfer
            return await _context.ProgressTransfers
                .Include(p => p.ProgressClients)
                    .ThenInclude(pc => pc.ClientTransfer)
                .OrderByDescending(p => p.EstimateTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<ProgressTransfer> CreateAsync(ProgressTransfer progressTransfer, CancellationToken cancellationToken = default)
        {
            await _context.ProgressTransfers.AddAsync(progressTransfer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return progressTransfer;
        }

        public async Task<ProgressTransfer> UpdateAsync(ProgressTransfer progressTransfer, CancellationToken cancellationToken = default)
        {
            _context.ProgressTransfers.Update(progressTransfer);
            await _context.SaveChangesAsync(cancellationToken);
            return progressTransfer;
        }

        public async Task<bool> UpdateStatusAsync(string id, string status, CancellationToken cancellationToken = default)
        {
            var progressTransfer = await _context.ProgressTransfers.FindAsync(new object[] { id }, cancellationToken);
            if (progressTransfer == null) return false;

            progressTransfer.Status = status;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
