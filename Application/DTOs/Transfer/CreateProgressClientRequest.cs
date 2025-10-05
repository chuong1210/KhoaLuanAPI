using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Transfer
{
    public class CreateProgressClientRequest
    {
        public int Sort { get; set; }
        public DateTime TimeTo { get; set; }
        public string ClientTransferId { get; set; }
    }
}
