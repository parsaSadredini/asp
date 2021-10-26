using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class LogicException : AppException
    {
        public LogicException (string message) : base(message , ApiResultStatusCode.LogicError)
        {

        }
    }
}
