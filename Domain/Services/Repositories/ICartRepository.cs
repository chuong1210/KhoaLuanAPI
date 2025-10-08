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
        Task<CartItem> UpdateItemQuantityAsync(string cartId, string skuId, int quantity, CancellationToken cancellationToken = default);
        Task<bool> RemoveItemAsync(string cartId, string skuId, CancellationToken cancellationToken = default);
        Task<bool> ClearCartAsync(string cartId, CancellationToken cancellationToken = default);
        Task<bool> UpdateItemSelectionAsync(string cartId, string skuId, bool isSelected, CancellationToken cancellationToken = default);
        Task<int> GetCartItemCountAsync(string cartId, CancellationToken cancellationToken = default);
        Task UpdateCachedProductInfoAsync(string skuId, string productName, string image, double price, string shopId);
    }
}
