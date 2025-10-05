using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationEvents.HttpClients.Dtos
{
    public class ProductDetailDto
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Details { get; set; } = string.Empty;

        public bool IsInStock { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedDate { get; set; }

        public string MainImage { get; set; } = string.Empty;

        public List<string> MediaUrls { get; set; } = new List<string>();  // Parse từ ProductMedia JSON

        public BrandDto Brand { get; set; } = new BrandDto();

        public CategoryDto Category { get; set; } = new CategoryDto();

        public ShopSummaryDto Shop { get; set; } = new ShopSummaryDto();

        public SkuDetailDto Sku { get; set; } = new SkuDetailDto();  // Chi tiết SKU chính

        public List<SkuDetailDto> AvailableSkus { get; set; } = new List<SkuDetailDto>();  // Nếu có variants

        public bool IsReturnable { get; set; }

        public decimal OriginalPrice { get; set; }  // Từ Sku

        public decimal? DiscountedPrice { get; set; }  // Nếu có promotion

        public int AvailableQuantity { get; set; }  // Từ Sku stock nếu có
    }



  


}
