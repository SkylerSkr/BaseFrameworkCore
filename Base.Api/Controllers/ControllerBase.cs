using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.SDK.Base;
using Base.SDK.Request.Test;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Base.Api.Controllers
{
    public class ControllerBase: Controller
    {
        public void SetJwtUidAndRole<T>(BaseApiRequest<T> request) where T:IApiResponse
        {
            request.JwtUid = Convert.ToInt32(HttpContext.Session.GetString("uid"));
            request.JwtRole = HttpContext.Session.GetString("role");
        }
    }
}
