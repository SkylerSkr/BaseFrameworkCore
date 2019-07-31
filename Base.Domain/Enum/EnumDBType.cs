using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Base.Domain.Enum
{
    /// <summary>
    /// 数据库枚举
    /// </summary>
    public enum EnumDBType
    {
        /// <summary>
        /// 日本
        /// </summary>
        [Description("日本")]
        Japan = 0,
        /// <summary>
        /// 新加坡
        /// </summary>
        [Description("新加坡")]
        Singapore = 1
    }
}
