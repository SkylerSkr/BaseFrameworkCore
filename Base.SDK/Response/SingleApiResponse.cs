using System;
using System.Collections.Generic;
using System.Text;
using Base.SDK.Base;

namespace Base.SDK.Response
{
    public class SingleApiResponse : BaseApiResponse
    {
        /// <summary>
        ///  响应对象
        /// </summary>
        public object Data { get; set; }
    }
}
