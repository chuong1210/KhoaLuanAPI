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

    public class CartRepository : ICartRepository
    {
        private readonly ShopDbContext _context;
        private readonly ILogger<CartRepository> _logger;

        public CartRepository(ShopDbContext context, ILogger<CartRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Cart> GetByUserProfileIdAsync(string userProfileId, CancellationToken cancellationToken = default)
        {
            return await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserProfileId == userProfileId, cancellationToken);
        }

        public async Task<Cart> GetOrCreateByUserProfileIdAsync(string userProfileId, CancellationToken cancellationToken = default)
        {
            var cart = await GetByUserProfileIdAsync(userProfileId, cancellationToken);

            if (cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid().ToString(),
                    UserProfileId = userProfileId,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    Items = new List<CartItem>()
                };

                await _context.Carts.AddAsync(cart, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Created new cart {CartId} for user {UserId}", cart.Id, userProfileId);
            }

            return cart;
        }

        public async Task<CartItem> AddItemAsync(CartItem cartItem, CancellationToken cancellationToken = default)
        {
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartItem.CartId && ci.SkuId == cartItem.SkuId, cancellationToken);

            if (existingItem != null)
            {
                existingItem.Quantity += cartItem.Quantity;
                existingItem.AddedDate = DateTime.UtcNow;
                existingItem.CachedProductName = cartItem.CachedProductName;
                existingItem.CachedProductImage = cartItem.CachedProductImage;
                existingItem.CachedPrice = cartItem.CachedPrice;
                existingItem.CachedShopId = cartItem.CachedShopId;
                existingItem.CachedAt = DateTime.UtcNow;

                _logger.LogInformation("Updated quantity for SKU {SkuId} in cart {CartId}, new quantity: {Quantity}",
                    cartItem.SkuId, cartItem.CartId, existingItem.Quantity);
            }
            else
            {
                await _context.CartItems.AddAsync(cartItem, cancellationToken);
                _logger.LogInformation("Added new item SKU {SkuId} to cart {CartId}", cartItem.SkuId, cartItem.CartId);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return existingItem ?? cartItem;
        }

        public async Task UpdateAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            cart.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<CartItem> UpdateItemQuantityAsync(string cartId, string skuId, int quantity, CancellationToken cancellationToken = default)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.SkuId == skuId, cancellationToken);

            if (cartItem == null) return null;

            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Updated quantity for SKU {SkuId} to {Quantity}", skuId, quantity);
            return cartItem;
        }

        public async Task<bool> RemoveItemAsync(string cartId, string skuId, CancellationToken cancellationToken = default)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.SkuId == skuId, cancellationToken);

            if (cartItem == null) return false;

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Removed SKU {SkuId} from cart {CartId}", skuId, cartId);
            return true;
        }

        public async Task<bool> ClearCartAsync(string cartId, CancellationToken cancellationToken = default)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync(cancellationToken);

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Cleared {Count} items from cart {CartId}", cartItems.Count, cartId);
            return true;
        }

        public async Task<bool> UpdateItemSelectionAsync(string cartId, string skuId, bool isSelected, CancellationToken cancellationToken = default)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.SkuId == skuId, cancellationToken);

            if (cartItem == null) return false;

            cartItem.IsSelected = isSelected;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<int> GetCartItemCountAsync(string cartId, CancellationToken cancellationToken = default)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .SumAsync(ci => ci.Quantity, cancellationToken);
        }

        public async Task UpdateCachedProductInfoAsync(string skuId, string productName, string image, double price, string shopId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.SkuId == skuId)
                .ToListAsync();

            foreach (var item in cartItems)
            {
                item.CachedProductName = productName;
                item.CachedProductImage = image;
                item.CachedPrice = price;
                item.CachedShopId = shopId;
                item.CachedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated cached product info for SKU {SkuId} in {Count} cart items", skuId, cartItems.Count);
        }
    }

}
