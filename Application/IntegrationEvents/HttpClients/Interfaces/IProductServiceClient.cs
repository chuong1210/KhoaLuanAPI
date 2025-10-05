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
        Task<ProductDetailDto> GetProductByIdAsync(string productId);
        Task<List<ProductDetailDto>> GetProductsByIdsAsync(List<string> productIds);
        Task<SkuDetailDto> GetSkuByIdAsync(string skuId);
        Task<List<SkuDetailDto>> GetSkusByIdsAsync(List<string> skuIds);
    }
}
