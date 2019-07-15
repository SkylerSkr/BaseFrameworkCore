using System;
using System.Collections.Generic;
using System.Text;

namespace Base.SDK.Base
{
    /// <summary>
    /// api response包裹
    /// </summary>
    public sealed class ApiResponseWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        public string Body { get; set; }

    }

    /// <summary>
    /// 前端响应对象
    /// </summary>
    public class FBaseApiResponse : BaseApiResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public object Data { get; set; }
    }

    /// <summary>
    /// 前端列表响应对象
    /// </summary>
    public class FListApiResponse : FBaseApiResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public int? TotalCount { get; set; }
    }

    /// <summary>
    /// api系统反馈基类
    /// </summary>
    public abstract class BaseApiResponse : IApiResponse
    {
        #region 系统级别错误验证
        /// <summary>
        /// 系统级别的参数，请业务代码不要使用 - 错误码
        /// </summary>
        public int? ErrCode { get; set; }

        /// <summary>
        /// 系统级别的参数，请业务代码不要使用 - 错误信息
        /// </summary>
        public string ErrMsg { get; set; }

        /// <summary>
        /// 系统级别的方法，请业务代码不要使用 - 获取失败信息
        /// </summary>
        /// <returns></returns>
        public string GetErrorMessage()
        {
            return ErrMsg;
        }

        /// <summary>
        /// 系统级别的方法，请业务代码不要使用 - 请求是否成功
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess
        {
            get { return ErrCode == null; }
        }

        /// <summary>
        /// 返回错误code
        /// </summary>
        /// <returns></returns>
        public int? GetErrCode()
        {
            return ErrCode;
        }
        #endregion

        #region 业务级别错误验证

        /// <summary>
        /// 业务错误信息
        /// </summary>
        public string BizErrorMsg { get; set; }

        /// <summary>
        /// 业务是否成功
        /// </summary>
        public bool IsBizSuccess
        {
            get { return string.IsNullOrEmpty(BizErrorMsg); }
        }
        #endregion
    }
}
