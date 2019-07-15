using System;
using System.Collections.Generic;
using System.Text;

namespace Base.SDK.Base
{
    /// <summary>
    /// 列表请求基类
    /// </summary>
    public abstract class BaseApiListRequest<TR> : BaseApiRequest<TR> where TR : IApiResponse
    {
        /// <summary>
        /// 页码，第一页为1，第二页为2，以此类推，null值为不执行分页查询
        /// </summary>
        public virtual int? PageNo { get; set; }

        /// <summary>
        /// 每页记录数，null值为不执行分页查询
        /// </summary>
        public virtual int? PageSize { get; set; }

        /// <summary>
        /// 起始数
        /// </summary>
        public virtual int? StartSize
        {
            get { return (PageNo - 1) * PageSize; }
        }
    }
}
