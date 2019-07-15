using System;
using System.Collections.Generic;
using System.Text;
using Base.SDK.Base;
using Base.SDK.Response;

namespace Base.SDK.Request.Test
{
    /// <summary>
    /// 保存
    /// </summary>
    public class TestSaveRequest : BaseApiRequest<SingleApiResponse>
    {
        public int? UID { get; set; }

        public string ULoginName { get; set; }
        public string ULoginPWD { get; set; }
        public string URealName { get; set; }
        public override string GetApiName()
        {
            return "Test/Save";
        }
    }
}
