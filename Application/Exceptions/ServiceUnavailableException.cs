using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class ServiceUnavailableException : Exception
    {
        public HttpStatusCode StatusCode { get; } = HttpStatusCode.ServiceUnavailable;

        public ServiceUnavailableException(string message) : base(message) { }

        public ServiceUnavailableException(string message, Exception innerException) : base(message, innerException) { }
    }
}
