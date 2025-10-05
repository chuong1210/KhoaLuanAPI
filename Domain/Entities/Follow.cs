using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Follow
    {
        public string ShopId { get; set; }
        public string UserProfileId { get; set; } // Reference only
        public DateTime CreatedDate { get; set; }

        public virtual Shop Shop { get; set; }
    }


}
