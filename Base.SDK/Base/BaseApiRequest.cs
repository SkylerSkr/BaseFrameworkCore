using System;
using System.Collections.Generic;
using System.Text;

namespace Base.SDK.Base
{
    /// <summary>
    /// 请求基类
    /// </summary>
    public abstract class BaseApiRequest<TR> : IApiRequest<TR> where TR : IApiResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 获取接口地址
        /// </summary>
        /// <returns></returns>
        public virtual string GetApiName() { return string.Empty; }

        /// <summary>
        /// 获取用户sessionid
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {
            return Token;
        }
    }
}
