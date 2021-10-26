using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class BadRequestException : AppException
    {
        public BadRequestException(string message) : base(message, ApiResultStatusCode.BadRequest)
        {

        }
    }
}
