using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetByUserProfileIdAsync(string userProfileId, CancellationToken cancellationToken = default);
        Task<Cart> GetOrCreateByUserProfileIdAsync(string userProfileId, CancellationToken cancellationToken = default);
        Task<CartItem> AddItemAsync(CartItem cartItem, CancellationToken cancellationToken = default);
        Task UpdateAsync(Cart cart, CancellationToken cancellationToken = default);
        Task<bool> RemoveItemAsync(string cartId, string skuId, CancellationToken cancellationToken = default);
        Task<bool> ClearCartAsync(string cartId, CancellationToken cancellationToken = default);

        // MỚI: Update cached product info từ Kafka event
        Task UpdateCachedProductInfoAsync(string skuId, string productName, string image, double price, string shopId);
    }
}
