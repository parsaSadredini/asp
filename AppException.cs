using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class AppException : Exception
    {
        public ApiResultStatusCode statusCode { get; set; }
        public AppException(string message,ApiResultStatusCode statuCode)
            :base(message)
        {
            this.statusCode = statusCode;
        }
    }
}
