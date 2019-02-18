using System;

namespace Infrastructure.NLoger
{
    public interface ICustomerLogging
    {
        void LogCustomerError(string errorMessage,string method,string logger,Exception ex);
        void LogCustomerInfo(string errorMessage,string method,string logger,Exception ex);
        void LogCustomerDebug(string errorMessage,string method,string logger,Exception ex);
        void LogCustomerTrace(string errorMessage,string method,string logger,Exception ex);

        void LogLoginInfo(string message,string method,string logger,Exception ex,string username,string uid);

    }
}