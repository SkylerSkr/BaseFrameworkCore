using Base.IBusinessService;
using Base.SDK.Model;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Base.Api.AuthHelper.OverWrite;
using Base.BusinessService;
using Base.Common.Token;

namespace Base.Api.AuthHelper.Policys
{
    /// <summary>
    /// 权限授权处理器 继承AuthorizationHandler ，并且需要一个权限必要参数
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// services 层注入
        /// </summary>
        private readonly ITestBiz _testBiz=new TestBiz();

        //public PermissionHandler(TestBiz testBiz)
        //{
        //    _testBiz = testBiz;
        //}


        // 重载异步处理程序
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // 将最新的角色和接口列表更新
            var data = _testBiz.GetRoleModule();
            var list = (from item in data
                        orderby item.UserId
                        select new PermissionItem
                        {
                            LinkUrl = item.LinkUrl,
                            UserId = item.UserId,
                        }).ToList();

            requirement.Permissions = list;


            //从AuthorizationHandlerContext转成HttpContext，以便取出表头信息
            var httpContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext)?.HttpContext;
            //请求Url
            var questUrl = httpContext.Request.Path.Value.ToLower();


            //权限中是否存在请求的url
            if (requirement.Permissions.GroupBy(g => g.LinkUrl).Any(w => w.Key?.ToLower() == questUrl))
            {
                // 获取当前用户的角色信息
                var currentUserRole = (from item in httpContext.User.Claims
                                        where item.Type == requirement.ClaimType
                                        select item.Value).FirstOrDefault();

                //验证权限
                if (currentUserRole==null || requirement.Permissions.Where(w => w.LinkUrl.ToLower() == questUrl).All(w => w.UserId != Convert.ToInt32(currentUserRole)) )
                {

                    context.Fail();
                    return;
                }
            }
            else
            {
                context.Fail();
                return;

            }

            //var xxx = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
            //var xx1 = httpContext.User.Claims.SingleOrDefault(s => s.Type == "exp")?.Value;
            ////判断过期时间
            //if ((httpContext.User.Claims.SingleOrDefault(s => s.Type == "exp")?.Value) != null && DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == "exp")?.Value) >= DateTime.Now)
            //{
            //    context.Succeed(requirement);
            //}
            //else
            //{
            //    context.Fail();
            //    return;
            //}

            ////判断没有登录时，是否访问登录的url,并且是Post请求，并且是form表单提交类型，否则为失败
            //if (!questUrl.Equals(requirement.LoginPath.ToLower(), StringComparison.Ordinal) && (!httpContext.Request.Method.Equals("POST")
            //   || !httpContext.Request.HasFormContentType))
            //{
            //    context.Fail();
            //    return;
            //}
            context.Succeed(requirement);
        }
    }
}
