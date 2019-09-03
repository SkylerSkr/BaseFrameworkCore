using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Base.SDK.Base;
using Base.SDK.Response;

namespace Base.SDK.Request.Test
{
    /// <summary>
    /// 获取单个
    /// </summary>
    public class TestGetRequest : BaseApiRequest<SingleApiResponse>
    {
        /// <summary>
        /// ID
        /// </summary>
        public int UID { get; set; }

        public override string GetApiName()
        {
            return "Values/Get";
        }
    }
}
