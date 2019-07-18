using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.Repository.Attribute;
using Base.Repository.Redis;
using Base.SDK.Response;
using Castle.DynamicProxy;

namespace Base.Api.Interceptor
{
    public class RedisCacheAOPInterceptor : IInterceptor
    {
        //通过注入的方式，把缓存操作接口通过构造函数注入
        public IRedisCacheManager Cache;
        public RedisCacheAOPInterceptor(IRedisCacheManager cache)
        {
            Cache = cache;
        }

        //Intercept方法是拦截的关键所在，也是IInterceptor接口中的唯一定义
        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            //对当前方法的特性验证
            var qCachingAttribute = method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(CachingAttribute)) as CachingAttribute;
            if (qCachingAttribute != null)
            {
                //获取自定义缓存键，这个和Memory内存缓存是一样的，不细说
                var cacheKey = CustomCacheKey(invocation);
                //核心1：注意这里和之前不同，是获取的string值，之前是object
                var cacheValue = Cache.GetValue(cacheKey);
                if (cacheValue != null)
                {
                    //将当前获取到的缓存值，赋值给当前执行方法
                    var type = invocation.Method.ReturnType;
                    var resultTypes = type.GenericTypeArguments;
                    if (type.FullName == "System.Void")
                    {
                        return;
                    }
                    object response = null;
                    if (typeof(Task).IsAssignableFrom(type))
                    {
                        //返回Task<T>
                        if (resultTypes.Any())
                        {
                            var resultType = resultTypes.FirstOrDefault();
                            // 核心1，直接获取 dynamic 类型
                            dynamic temp = Newtonsoft.Json.JsonConvert.DeserializeObject(cacheValue, resultType);
                            //dynamic temp = System.Convert.ChangeType(cacheValue, resultType);
                            // System.Convert.ChangeType(Task.FromResult(temp), type);
                            response = Task.FromResult(temp);

                        }
                        else
                        {
                            //Task 无返回方法 指定时间内不允许重新运行
                            response = Task.Yield();
                        }
                    }
                    else
                    {
                        // 核心2，要进行 ChangeType
                        //response = Convert.ChangeType(Cache.Get<object>(cacheKey), type);
                        if (type == typeof(SingleApiResponse))
                        {
                            response = Cache.Get<SingleApiResponse>(cacheKey);
                        }
                        else if (type == typeof(ListApiResponse))
                        {
                            response = Cache.Get<ListApiResponse>(cacheKey);
                        }

                    }

                    invocation.ReturnValue = response;
                    return;
                }
                //去执行当前的方法
                invocation.Proceed();

                //存入缓存
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    object response;

                    //Type type = invocation.ReturnValue?.GetType();
                    var type = invocation.Method.ReturnType;
                    if (type != null && typeof(Task).IsAssignableFrom(type))
                    {
                        var resultProperty = type.GetProperty("Result");
                        response = resultProperty.GetValue(invocation.ReturnValue);
                    }
                    else
                    {
                        response = invocation.ReturnValue;
                    }
                    if (response == null) response = string.Empty;
                    // 核心5：将获取到指定的response 和特性的缓存时间，进行set操作
                    Cache.Set(cacheKey, response, TimeSpan.FromMilliseconds(qCachingAttribute.AbsoluteExpiration));
                }
            }
            else
            {
                invocation.Proceed();//直接执行被拦截方法
            }
        }
        /// <summary>
        /// 自定义缓存的key
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        protected string CustomCacheKey(IInvocation invocation)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            var methodArguments = invocation.Arguments.Select(GetArgumentValue).Take(3).ToList();//获取参数列表，最多三个

            string key = $"{typeName}:{methodName}:";
            foreach (var param in methodArguments)
            {
                key = $"{key}{param}:";
            }

            return key.TrimEnd(':');
        }

        /// <summary>
        /// object 转 string
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        protected static string GetArgumentValue(object arg)
        {
            if (arg is DateTime || arg is DateTime?)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");

            if (arg is string || arg is ValueType || arg is Nullable)
                return arg.ToString();

            if (arg != null)
            {
                if (arg.GetType().IsClass)
                {
                    return Common.Helper.MD5Helper.MD5Encrypt16(Newtonsoft.Json.JsonConvert.SerializeObject(arg));
                }
            }

            return string.Empty;
        }

    }
}
