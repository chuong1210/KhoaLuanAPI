using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Cart
{
    public class BatchUpdateSelectionRequest
    {
        public List<string> SkuIds { get; set; }
        public bool IsSelected { get; set; }
    }
}
