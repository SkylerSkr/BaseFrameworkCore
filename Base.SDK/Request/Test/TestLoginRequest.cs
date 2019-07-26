using System;
using System.Collections.Generic;
using System.Text;
using Base.SDK.Base;
using Base.SDK.Response;

namespace Base.SDK.Request.Test
{
    /// <summary>
    /// 获取单个
    /// </summary>
    public class TestLoginRequest : BaseApiRequest<SingleApiResponse>
    {
        /// <summary>
        /// ID
        /// </summary>
        public string userRole { get; set; }

        public override string GetApiName()
        {
            return "Test/Login";
        }
    }
}
