using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Exceptions
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; } = new List<string>();

        public ValidationException() : base("Ocurrieron uno o más errores de validación.")
        {
        }

        public ValidationException(string message) : base(message)
        {
            Errors.Add(message);
        }

        public ValidationException(IEnumerable<string> errors) : this()
        {
            Errors.AddRange(errors);
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }
        public override string ToString()
        {
            return $"{base.ToString()}, Errores: {string.Join("; ", Errors)}";
        }
    }
}
