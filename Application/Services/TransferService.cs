using Application.Cache.Interfaces;
using Application.DTOs.Transfer;
using Application.IntegrationEvents.HttpClients.Interfaces;
using Application.Interfaces.Identity;
using Application.Interfaces.Services;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Services.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TransferService : ITransferService
    {
        private readonly IProgressTransferRepository _progressTransferRepository;
        private readonly IOfflineTransactionRepository _offlineTransactionRepository;
        private readonly IProfileServiceClient _profileClient;
        private readonly ICacheService _cache;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ILogger<TransferService> _logger;

        public TransferService(
            IProgressTransferRepository progressTransferRepository,
            IOfflineTransactionRepository offlineTransactionRepository,
            IProfileServiceClient profileClient,
            ICacheService cache,
            ICurrentUserService currentUserService,
            IMapper mapper,
            ILogger<TransferService> logger)
        {
            _progressTransferRepository = progressTransferRepository;
            _offlineTransactionRepository = offlineTransactionRepository;
            _profileClient = profileClient;
            _cache = cache;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<ProgressTransferDto>> GetProgressTransferByIdAsync(
            string id, CancellationToken cancellationToken = default)
        {
            var progressTransfer = await _progressTransferRepository.GetByIdAsync(id, cancellationToken);
            if (progressTransfer == null)
                return Result<ProgressTransferDto>.Failure("Thông tin vận chuyển không tồn tại", 404);

            var dto = _mapper.Map<ProgressTransferDto>(progressTransfer);
            return Result<ProgressTransferDto>.Success(dto, 200);
        }

        public async Task<Result<ProgressTransferDto>> GetProgressTransferByOrderShopIdAsync(
            string orderShopId, CancellationToken cancellationToken = default)
        {
            var progressTransfer = await _progressTransferRepository.GetByOrderShopIdAsync(orderShopId, cancellationToken);
            if (progressTransfer == null)
                return Result<ProgressTransferDto>.Failure("Thông tin vận chuyển không tồn tại", 404);

            var dto = _mapper.Map<ProgressTransferDto>(progressTransfer);
            return Result<ProgressTransferDto>.Success(dto, 200);
        }

        public async Task<Result<ProgressTransferDto>> CreateProgressTransferAsync(
            CreateProgressTransferRequest request, CancellationToken cancellationToken = default)
        {
            if (!_currentUserService.IsInRole("Admin") && !_currentUserService.IsInRole("Shipper"))
                return Result<ProgressTransferDto>.Forbidden();

            var progressTransfer = _mapper.Map<ProgressTransfer>(request);

            // Map progress clients
            if (request.ProgressClients != null && request.ProgressClients.Any())
            {
                progressTransfer.ProgressClients = request.ProgressClients.Select(pc => new ProgressClient
                {
                    Sort = pc.Sort,
                    TimeTo = pc.TimeTo,
                    ProgressTransferId = progressTransfer.Id,
                    ClientTransferId = pc.ClientTransferId
                }).ToList();
            }

            var created = await _progressTransferRepository.CreateAsync(progressTransfer, cancellationToken);
            var dto = _mapper.Map<ProgressTransferDto>(created);

            _logger.LogInformation("Created progress transfer {Id} for order shop {OrderShopId}",
                created.Id, created.OrderShopId);

            return Result<ProgressTransferDto>.Success(dto, 201);
        }

        public async Task<Result<bool>> UpdateProgressTransferStatusAsync(
            string id, UpdateProgressTransferStatusRequest request, CancellationToken cancellationToken = default)
        {
            if (!_currentUserService.IsInRole("Admin") && !_currentUserService.IsInRole("Shipper"))
                return Result<bool>.Forbidden();

            var result = await _progressTransferRepository.UpdateStatusAsync(id, request.Status, cancellationToken);

            if (!result)
                return Result<bool>.Failure("Thông tin vận chuyển không tồn tại", 404);

            _logger.LogInformation("Updated progress transfer {Id} status to {Status}", id, request.Status);

            return Result<bool>.Success(true, 200);
        }

        public async Task<Result<OfflineTransactionDto>> CreateOfflineTransactionAsync(
            CreateOfflineTransactionRequest request, CancellationToken cancellationToken = default)
        {
            if (!_currentUserService.IsInRole("Shipper"))
                return Result<OfflineTransactionDto>.Forbidden();

            var transaction = _mapper.Map<OfflineTransaction>(request);
            var created = await _offlineTransactionRepository.CreateAsync(transaction, cancellationToken);
            var dto = _mapper.Map<OfflineTransactionDto>(created);

            _logger.LogInformation("Created COD transaction {Id} for order shop {OrderShopId} amount {Amount}",
                created.Id, created.OrderShopId, created.Amount);

            return Result<OfflineTransactionDto>.Success(dto, 201);
        }
    
}
}
