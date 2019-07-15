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

            var result = new ExceptionResponse
            {
                ErrCode = 500,
                ErrMsg = context.Exception.Message,
                BizErrorMsg = context.Exception.Message
            };
            context.HttpContext.Response.StatusCode = 200;
            context.HttpContext.Response.ContentType = "application/json;charset=utf-8";
            context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(result));
            base.OnException(context);
        }
    }
}
