﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Common.Attribute
{ /// <summary>
    /// 这个Attribute就是使用时候的验证，把它添加到要缓存数据的方法中，即可完成缓存的操作。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CachingAttribute : System.Attribute
    {
        /// <summary>
        /// 缓存绝对过期时间（毫秒）
        /// </summary>
        public int AbsoluteExpiration { get; set; } = 300;

    }
}
