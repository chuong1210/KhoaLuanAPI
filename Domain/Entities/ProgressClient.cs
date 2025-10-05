using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProgressClient
    {
        public int Sort { get; set; }
        public DateTime TimeTo { get; set; }
        public string ProgressTransferId { get; set; }
        public string ClientTransferId { get; set; }

        public virtual ProgressTransfer ProgressTransfer { get; set; }
        public virtual ClientTransfer ClientTransfer { get; set; }
    }
}
