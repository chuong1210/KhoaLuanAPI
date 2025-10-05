using Domain.Entities;
using Domain.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OfflineTransactionRepository : IOfflineTransactionRepository
    {
        private readonly ShopDbContext _context;

        public OfflineTransactionRepository(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<OfflineTransaction> CreateAsync(OfflineTransaction transaction, CancellationToken cancellationToken = default)
        {
            await _context.OfflineTransactions.AddAsync(transaction, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return transaction;
        }

        public async Task<List<OfflineTransaction>> GetByOrderShopIdAsync(string orderShopId, CancellationToken cancellationToken = default)
        {
            return await _context.OfflineTransactions
                .Where(t => t.OrderShopId == orderShopId)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync(cancellationToken);
        }
    }
}
