using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using D3.Blog.Domain.Infrastructure;
using Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

namespace Infrastructure.AOP
{
    /// <summary>
    /// 日志AOP
    /// </summary>
    public class BlogLogAOP : IInterceptor
    {
        internal Serilog.ILogger _logger ;
        internal IUser _user;

        public BlogLogAOP(Serilog.ILogger logger,IUser user)
        {
            _logger = logger;
            _user = user;
        }


        public void Intercept(IInvocation invocation)
        {
            //记录被拦截方法信息的日志信息
            var dataIntercept = $"当前执行方法：{ invocation.Method.Name} " +
                                $"参数是： {string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())} \r\n";
            try
            {
                //在被拦截的方法执行完毕后 继续执行当前方法，注意是被拦截的是异步的
                invocation.Proceed();
                dataIntercept += ($"执行完毕，返回结果：{invocation.ReturnValue}");
                _logger.CustomInformation(_user==null?"":_user.Name,"","","",dataIntercept);
            }
            catch (Exception e)
            {
                //执行的 service 中，出现异常
                dataIntercept += ($"方法执行中出现异常：{e.Message+e.InnerException}");
                _logger.CustomInformation(_user==null?"":_user.Name,"","","",dataIntercept);
            }
        }
    }
}