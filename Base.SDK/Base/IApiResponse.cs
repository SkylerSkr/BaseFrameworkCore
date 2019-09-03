using System;
using System.Collections.Generic;
using System.Text;

namespace Base.SDK.Base
{
    /// <summary>
    /// api响应接口
    /// </summary>
    public interface IApiResponse
    {
        /// <summary>
        /// 系统级别的参数，请业务代码不要使用 - 错误码
        /// </summary>
        int? ErrCode { get; set; }

        /// <summary>
        /// 系统级别的参数，请业务代码不要使用 - 错误信息
        /// </summary>
        string ErrMsg { get; set; }

        ///// <summary>
        ///// 请求是否成功
        ///// </summary>
        ///// <returns></returns>
        bool IsSuccess { get; }

        /// <summary>
        /// 业务是否成功
        /// </summary>
        /// <returns></returns>
        bool IsBizSuccess { get; }
    }
}
