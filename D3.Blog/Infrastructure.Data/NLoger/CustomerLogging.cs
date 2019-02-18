using System;
using D3.Blog.Domain.Infrastructure;
using NLog;

namespace Infrastructure.NLoger
{
    /// <summary>
    /// 自定义日志记录
    /// </summary>
    public class CustomerLogging:ICustomerLogging
    {
        private readonly Logger nlog = LogManager.GetCurrentClassLogger(); //获得日志实例;

        internal IUser _user;

        public CustomerLogging(IUser user)
        {
            _user = user;
        }

        /// <summary>
        /// 自定义debug日志
        /// </summary>
        /// <param name="errorMessage">信息</param>
        /// <param name="ipaddress">请求IP地址</param>
        /// <param name="method">执行方法</param>
        /// <param name="logger">那个类下边的</param>
        public void LogCustomerDebug(string errorMessage, string method, string logger,Exception ex)
        {
            LogEventInfo ei = new LogEventInfo();
            if (_user!=null)
            {
                ei.Properties["userName"] = _user.Name;
                ei.Properties["userId"] =_user.Id;
                ei.Properties["ipAddress"] = _user.ClientIP;
            }
            ei.Properties["logMessage"] = errorMessage;
            ei.Properties["logger"] = "ValuesController";
            ei.Properties["Method"] = "ValuesController.Get";
            if (ex!=null)
            {
                ei.Exception = ex;
            }
            ei.Level = LogLevel.Debug;
            nlog.Log(ei);
        }
        /// <summary>
        /// 自定义Error日志
        /// </summary>
        /// <param name="errorMessage">信息</param>
        /// <param name="ipaddress">请求IP地址</param>
        /// <param name="method">执行方法</param>
        /// <param name="logger">那个类下边的</param>
        public void LogCustomerError(string errorMessage, string method, string logger,Exception ex)
        {
            LogEventInfo ei = new LogEventInfo();
            if (_user!=null)
            {
                ei.Properties["userName"] = _user.Name;
                ei.Properties["userId"] =_user.Id;
                ei.Properties["ipAddress"] = _user.ClientIP;
            }
            ei.Properties["logMessage"] = errorMessage;
            ei.Properties["logger"] = logger;
            ei.Properties["Method"] =method;
            if (ex!=null)
            {
                ei.Exception = ex;
            }
            ei.Level = LogLevel.Error;
            nlog.Log(ei);
        }
        /// <summary>
        /// 自定义Info日志
        /// </summary>
        /// <param name="errorMessage">信息</param>
        /// <param name="ipaddress">请求IP地址</param>
        /// <param name="method">执行方法</param>
        /// <param name="logger">那个类下边的</param>
        public void LogCustomerInfo(string errorMessage, string method, string logger,Exception ex)
        {
            LogEventInfo ei = new LogEventInfo();
            if (_user!=null)
            {
                ei.Properties["userName"] = _user.Name;
                ei.Properties["userId"] =_user.Id;
                ei.Properties["ipAddress"] = _user.ClientIP;
            }
            ei.Properties["logMessage"] = errorMessage;
            ei.Properties["logger"] = logger;
            ei.Properties["Method"] =method;
            if (ex!=null)
            {
                ei.Exception = ex;
            }
            ei.Level = LogLevel.Info;
            nlog.Log(ei);
        }
        /// <summary>
        /// 自定义Trace日志
        /// </summary>
        /// <param name="errorMessage">信息</param>
        /// <param name="ipaddress">请求IP地址</param>
        /// <param name="method">执行方法</param>
        /// <param name="logger">那个类下边的</param>
        public void LogCustomerTrace(string errorMessage,  string method, string logger,Exception ex)
        {
            LogEventInfo ei = new LogEventInfo();
            if (_user!=null)
            {
                ei.Properties["userName"] = _user.Name;
                ei.Properties["userId"] =_user.Id;
                ei.Properties["ipAddress"] = _user.ClientIP;
            }
            ei.Properties["logMessage"] = errorMessage;
            ei.Properties["logger"] = logger;
            ei.Properties["Method"] =method;
            if (ex!=null)
            {
                ei.Exception = ex;
            }
            ei.Level = LogLevel.Trace;
            nlog.Log(ei);
        }
        /// <summary>
        /// 用户登录注销日志记录
        /// </summary>
        /// <param name="message"></param>
        /// <param name="method"></param>
        /// <param name="logger"></param>
        /// <param name="ex"></param>
        /// <param name="username"></param>
        /// <param name="uid"></param>
        public void LogLoginInfo(string message, string method, string logger, Exception ex, string username, string uid)
        {
            LogEventInfo ei = new LogEventInfo();
            ei.Properties["userName"] =username;
            ei.Properties["userId"] =uid;
            if (_user!=null)
            {
                ei.Properties["ipAddress"] = _user.ClientIP;
            }
            ei.Properties["logMessage"] = message;
            ei.Properties["logger"] = logger;
            ei.Properties["Method"] =method;
            if (ex!=null)
            {
                ei.Exception = ex;
            }
            ei.Level = LogLevel.Info;
            nlog.Log(ei);
        }
    }
}