using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class NotFoundException: AppException
    {
        public NotFoundException() : this("آیتم مورد نظر پیدا نشد")
        {

        }
        public NotFoundException(string message) : base(message , ApiResultStatusCode.NotFound)
        {

        }
    }
}
