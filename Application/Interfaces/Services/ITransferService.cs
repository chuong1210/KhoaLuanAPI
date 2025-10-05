using Application.DTOs.Transfer;
using Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface ITransferService
    {
        Task<Result<ProgressTransferDto>> GetProgressTransferByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<Result<ProgressTransferDto>> GetProgressTransferByOrderShopIdAsync(string orderShopId, CancellationToken cancellationToken = default);
        Task<Result<ProgressTransferDto>> CreateProgressTransferAsync(CreateProgressTransferRequest request, CancellationToken cancellationToken = default);
        Task<Result<bool>> UpdateProgressTransferStatusAsync(string id, UpdateProgressTransferStatusRequest request, CancellationToken cancellationToken = default);
        Task<Result<OfflineTransactionDto>> CreateOfflineTransactionAsync(CreateOfflineTransactionRequest request, CancellationToken cancellationToken = default);
    }
}
