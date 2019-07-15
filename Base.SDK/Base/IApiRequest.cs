using System;
using System.Collections.Generic;
using System.Text;

namespace Base.SDK.Base
{
    /// <summary>
    /// api请求接口
    /// </summary>
    public interface IApiRequest<out TR> where TR : IApiResponse
    {
        /// <summary>
        /// 获取接口地址
        /// </summary>
        string GetApiName();

        /// <summary>
        /// 获取加密签名
        /// </summary>
        /// <returns></returns>
        string GetToken();
    }
}
