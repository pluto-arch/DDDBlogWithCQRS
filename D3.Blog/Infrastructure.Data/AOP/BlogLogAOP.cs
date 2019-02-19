using System;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using D3.Blog.Domain.Infrastructure;
using Infrastructure.JsonHelper;
using Infrastructure.Logging;
using Infrastructure.NLoger;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NLog.Targets;
using Serilog;
using Serilog.Core;

namespace Infrastructure.AOP
{
    /// <summary>
    /// 日志AOP
    /// </summary>
    public class BlogLogAOP : IInterceptor
    {
        internal ICustomerLogging _logger ;
        internal IUser _user;

        public BlogLogAOP(IUser user,ICustomerLogging logger)
        {
            _logger = logger;
            _user = user;
        }


        public void Intercept(IInvocation invocation)
        {
            //记录被拦截方法信息的日志信息

            LogMessageJson logJson=new LogMessageJson();
            logJson.Method = invocation.Method.Name;
            logJson.Parametes = invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray();
            logJson.ReturnValue = invocation.ReturnValue==null?"":invocation.ReturnValue.ToString();
            try
            {
                //在被拦截的方法执行完毕后 继续执行当前方法，注意是被拦截的是异步的
                invocation.Proceed();
                logJson.ErrorMessage = "";
                var resjson = NewtonsoftJsonHelper.SerializeObject(logJson);
                _logger.LogCustomerInfo(resjson,invocation.Method.Name,invocation.Method.Name,null);
            }
            catch (Exception e)
            {
                //执行的 service 中，出现异常
                if (e.InnerException != null)
                {
                    logJson.ErrorMessage = e.Message + "] [" + e.InnerException.Message;
                }
                else
                {
                    logJson.ErrorMessage = e.Message;
                }
                var resjson = NewtonsoftJsonHelper.SerializeObject(logJson);
                _logger.LogCustomerError(resjson,invocation.Method.Name,invocation.Method.Name,e);
            }
        }
    }

    /// <summary>
    /// 将日志信息转为json格式
    /// </summary>
    class LogMessageJson
    {
        public string Method { get; set; }
        public string ErrorMessage { get; set; }
        public string[] Parametes { get; set; }
        public string ReturnValue { get; set; }

    }

}