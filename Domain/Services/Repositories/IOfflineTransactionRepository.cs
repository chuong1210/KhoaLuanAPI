using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Repositories
{
    public interface IOfflineTransactionRepository
    {
        Task<OfflineTransaction> CreateAsync(OfflineTransaction transaction, CancellationToken cancellationToken = default);
        Task<List<OfflineTransaction>> GetByOrderShopIdAsync(string orderShopId, CancellationToken cancellationToken = default);
    }
}

