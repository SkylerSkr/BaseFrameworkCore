using System;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Base.Api.Log;
using Base.SDK.Base;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Base.Api.Filters
{
    public class ApiActionFilterAttribute : ActionFilterAttribute
    {
        //public HashSet<string> WhiteListURL { get; set; } = new HashSet<string>(new[] { "values/login" });

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
            if (context.HttpContext.Request.Body.CanSeek)
            {
                using (var requestSm = context.HttpContext.Request.Body)
                {
                    requestSm.Position = 0;
                    var reader = new StreamReader(requestSm, Encoding.UTF8);
                    var requestBody = reader.ReadToEnd();
                    var requestMethod = context.HttpContext.Request.Method;
                    var requestHost = context.HttpContext.Request.Host.ToString();
                    var requestPath = context.HttpContext.Request.Path;
                    LogHelper.Info($"\r\nMethod:{requestMethod} \r\nHost:{requestHost} \r\nPath:{requestPath} \r\nBody:{requestBody} \r\n");
                    ////账户认证
                    //var reqObj = JsonConvert.DeserializeObject<dynamic>(requestBody);
                    ////不在白名单内
                    //if (!WhiteListURL.Contains(requestPath.ToString().ToLower().Replace(@"/api/","")))
                    //{
                    //    //验证api用户是否合法
                    //    if (false)
                    //    {
                    //        throw new Exception("身份验证失败。");
                    //    }
                    //}
                }
            }
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {

            base.OnResultExecuted(context);
        }

    }
}
