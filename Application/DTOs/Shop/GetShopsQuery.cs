using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Shop
{
    public class GetShopsQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 30;
        public string SearchTerm { get; set; }
        public bool Status { get; set; }

    }
}
