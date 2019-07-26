using Base.Api.Log;
using Base.SDK.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.IO;

namespace Base.Api.Filters
{
    public class ApiErrorFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.HttpContext.Request.Body.CanSeek)
            {
                using (var requestSm = context.HttpContext.Request.Body)
                {
                    var requestMethod = context.HttpContext.Request.Method;
                    var requestHost = context.HttpContext.Request.Host.ToString();
                    var requestPath = context.HttpContext.Request.Path;
                    var exception = context.Exception.Message;
                    LogHelper.Error($"\r\nMethod:{requestMethod} \r\nHost:{requestHost} \r\nPath:{requestPath} \r\nException:{exception} \r\n");
                }
            }

            base.OnException(context);
        }
    }
}
