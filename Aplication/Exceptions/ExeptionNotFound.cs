using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Exceptions
{
    public class ExeptionNotFound : Exception
    {
        public ExeptionNotFound(string message) : base(message)
        {
        }
        public ExeptionNotFound(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
