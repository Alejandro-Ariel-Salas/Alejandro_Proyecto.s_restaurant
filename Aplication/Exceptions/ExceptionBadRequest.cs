using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Exceptions
{
    public class ExceptionBadRequest : Exception
    {
        public ExceptionBadRequest(string message) : base(message)
        {
        }
        public ExceptionBadRequest(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
