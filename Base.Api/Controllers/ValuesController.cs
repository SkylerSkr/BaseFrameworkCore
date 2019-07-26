using Base.Api.AuthHelper.OverWrite;
using Base.IBusinessService;
using Base.Repository;
using Base.SDK.Request;
using Base.SDK.Request.Test;
using Base.SDK.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Base.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        public ITestBiz TestBiz { get; set; }

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public SingleApiResponse Login([FromBody]TestLoginRequest req)
        {
            TokenModelJwt tokenModel = new TokenModelJwt { Uid = 1, Role = req.userRole };
            var jwtStr = JwtHelper.IssueJwt(tokenModel);//登录，获取到一定规则的 Token 令牌
            return new SingleApiResponse(){Data = jwtStr };
        }

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Get")]
        public SingleApiResponse Get([FromBody]TestGetRequest req)
        {
            return TestBiz.Get(req);
        }

        /// <summary>
        /// 保存 Insert和Edit
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Save")]
        public SingleApiResponse Save([FromBody]TestSaveRequest req)
        {
            return TestBiz.Save(req);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        [Authorize(Roles = "Admin")]
        public SingleApiResponse Delete([FromBody]TestSaveRequest req)
        {
            return TestBiz.Delete(req);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("GetList")]
        [Authorize(Policy = "SystemOrAdmin")]
        public ListApiResponse GetList([FromBody]TestGetListRequest req)
        {
            return TestBiz.GetList(req);
        }
    }
}
