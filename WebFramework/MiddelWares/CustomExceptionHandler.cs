using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebFramework.Api;
using Common;
using Newtonsoft.Json;
using Common.Exceptions;
using Microsoft.AspNetCore.Builder;

namespace WebFramework.MiddelWares
{
    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandler>();
        }
    }
    public class CustomExceptionHandler 
    {
        
        private readonly RequestDelegate next;
        public CustomExceptionHandler(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }catch (AppException x)
            {
                var result = new ApiResult(false, x.ApiStatusCode,x.Message);
                var jsonResult = JsonConvert.SerializeObject(result);
                await context.Response.WriteAsync(jsonResult);
            }
            catch (Exception ex)
            {
                var result = new ApiResult(false, ApiResultStatusCode.ServerError);
                var jsonResult = JsonConvert.SerializeObject(result);
                await context.Response.WriteAsync(jsonResult);
            }
        }
    }
}
