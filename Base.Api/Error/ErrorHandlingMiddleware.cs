using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.SDK.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Base.Api.Error
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var msg = "";
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var statusCode = context.Response.StatusCode;
                if (ex is ArgumentException)
                {
                    statusCode = 200;
                }
                msg = ex.Message;
            }
            finally
            {
                var statusCode = context.Response.StatusCode;
                if (statusCode == 401)
                {
                    msg = "未授权";
                    await HandleExceptionAsync(context, statusCode, msg);
                }
                else if (statusCode == 404)
                {
                    msg = "未找到服务";
                    await HandleExceptionAsync(context, statusCode, msg);
                }
                else if (statusCode == 502)
                {
                    msg = "请求错误";
                    await HandleExceptionAsync(context, statusCode, msg);
                }
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string msg)
        {
            var result = JsonConvert.SerializeObject(new ExceptionResponse
            {
                ErrCode = statusCode,
                ErrMsg = msg,
                BizErrorMsg = msg
            });

            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(result);
        }
    }
    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
    //context.Response.WriteAsync(result);

}
