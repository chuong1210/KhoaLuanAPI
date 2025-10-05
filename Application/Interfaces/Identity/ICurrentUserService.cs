using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Identity
{
    public interface  ICurrentUserService
    {
       public string UserId { get; }
        public string? Username { get; }

        public string? UserProfileId { get; }

        public bool IsInRole(string role);
        public bool  IsAuthenticated { get; }

        //public string? Type { get; }

        //public int? StaffId { get; }

        //public int? CustomerId { get; }
    }
}
