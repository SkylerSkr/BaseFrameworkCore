using System;
using System.Collections.Generic;
using System.Text;
using Base.SDK.Base;
using Base.SDK.Response;

namespace Base.SDK.Request.Test
{
    /// <summary>
    /// 获取列表
    /// </summary>
    public class TestGetListRequest : BaseApiListRequest<ListApiResponse>
    {
        public int? UID { get; set; }
        public string ULoginName { get; set; }
        public string ULoginPWD { get; set; }
        public string URealName { get; set; }
        public override string GetApiName()
        {
            return "Test/GetList";
        }
    }
}
