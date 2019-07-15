using System;
using System.Collections.Generic;
using System.Text;
using Base.SDK.Base;
using Base.SDK.Response;

namespace Base.SDK.Request.Test
{
    /// <summary>
    /// 删除
    /// </summary>
    public class TestDeleteRequest : BaseApiRequest<SingleApiResponse>
    {
        public int UID { get; set; }

        public override string GetApiName()
        {
            return "Test/Delete";
        }
    }
}
