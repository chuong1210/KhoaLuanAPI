using Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public abstract class HardDeleteEntity : ISoftDelete
    {
        public int Id { get; set; } = default!;
        public bool IsDeleted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? DeletedDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string DeletedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
