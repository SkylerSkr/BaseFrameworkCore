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
        /// 获取错误信息
        /// </summary>
        /// <returns></returns>
        string GetErrorMessage();

        ///// <summary>
        ///// 请求是否成功
        ///// </summary>
        ///// <returns></returns>
        //bool IsSuccess();

        /// <summary>
        /// 获取错误code
        /// </summary>
        /// <returns></returns>
        int? GetErrCode();
    }
}
