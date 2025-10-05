using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Interfaces
{
    public abstract class BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }


    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedDate { get; set; }
        string DeletedBy { get; set; }
    }
}
