using System;
using System.Collections.Generic;
using System.Text;

namespace Base.SDK.Response
{
    public class ListApiResponse : SingleApiResponse
    {
        /// <summary>
        /// 总数量
        /// </summary>
        public int? TotalCount { get; set; }
    }
}
