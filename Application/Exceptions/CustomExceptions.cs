using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.") { }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = "Unauthorized access") : base(message) { }
    }

    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message = "Forbidden access") : base(message) { }
    }

    public class ValidationException : Exception
    {
        public List<string> Errors { get; }

        public ValidationException(List<string> errors) : base("One or more validation errors occurred")
        {
            Errors = errors;
        }

        public ValidationException(string error) : base(error)
        {
            Errors = new List<string> { error };
        }
    }

    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}
