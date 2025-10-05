using Domain.Entities;
using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Repositories
{
    public interface IProgressTransferRepository
    {
        Task<ProgressTransfer> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<ProgressTransfer> GetByOrderShopIdAsync(string orderShopId, CancellationToken cancellationToken = default);
        Task<List<ProgressTransfer>> GetByUserProfileIdAsync(string userProfileId, CancellationToken cancellationToken = default);
        Task<ProgressTransfer> CreateAsync(ProgressTransfer progressTransfer, CancellationToken cancellationToken = default);
        Task<ProgressTransfer> UpdateAsync(ProgressTransfer progressTransfer, CancellationToken cancellationToken = default);
        Task<bool> UpdateStatusAsync(string id, string status, CancellationToken cancellationToken = default);
    }
}
