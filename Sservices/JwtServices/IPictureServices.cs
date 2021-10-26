using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sservices.JwtServices
{
    public interface IPictureServices
    {
        string uploadFile(IFormFile file);

    }
}
