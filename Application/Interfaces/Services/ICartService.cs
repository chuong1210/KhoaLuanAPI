using Application.DTOs.Cart;
using Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface ICartService
    {
        Task<Result<CartDto>> GetMyCartAsync(CancellationToken cancellationToken = default);
        Task<Result<CartDto>> AddToCartAsync(AddToCartRequest request, CancellationToken cancellationToken = default);
        Task<Result<CartDto>> UpdateCartItemAsync(string skuId, UpdateCartItemRequest request, CancellationToken cancellationToken = default);
        Task<Result<bool>> RemoveCartItemAsync(string skuId, CancellationToken cancellationToken = default);
        Task<Result<bool>> ClearCartAsync(CancellationToken cancellationToken = default);
        Task<Result<bool>> UpdateItemSelectionAsync(string skuId, UpdateCartItemSelectionRequest request, CancellationToken cancellationToken = default);
        Task<Result<bool>> BatchUpdateSelectionAsync(BatchUpdateSelectionRequest request, CancellationToken cancellationToken = default);
        Task<Result<int>> GetCartItemCountAsync(CancellationToken cancellationToken = default);
    }

}
