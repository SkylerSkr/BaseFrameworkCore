using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Castle.DynamicProxy;

namespace Base.Api.Interceptor
{
    /// <summary>
    /// 事务拦截器
    /// </summary>
    public class TransactionInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                //在被拦截的方法执行完毕后 继续执行
                invocation.Proceed();
                scope.Complete();
            }
        }
    }
}
