using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationEvents.HttpClients.Dtos;
namespace Application.IntegrationEvents.HttpClients.Interfaces
{
    public interface IProductServiceClient
    {
        // SKU operations
        Task<SkuDetailDto> GetSkuByIdAsync(string skuId);
        Task<List<SkuDetailDto>> GetSkusByIdsAsync(List<string> skuIds);
        Task<bool> CheckSkuStockAsync(string skuId, int quantity);
        Task<bool> ReserveStockAsync(string skuId, int quantity);
        Task<bool> ReleaseStockAsync(string skuId, int quantity);

        // Product operations
        Task<ProductDetailDto> GetProductByIdAsync(string productId);
        Task<List<ProductDetailDto>> GetProductsByShopIdAsync(string shopId, int pageNumber, int pageSize);
        Task<bool> ProductExistsAsync(string productId);

        // Category operations
        Task<CategoryDto> GetCategoryByIdAsync(string categoryId);
        Task<List<CategoryDto>> GetCategoriesAsync();
    }
}
