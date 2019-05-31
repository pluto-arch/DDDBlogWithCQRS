using System;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DiagnosticAdapter;

namespace Infrastructure.Tools
{
    public class CommandListener : IObserver<DiagnosticListener>
    {

        private readonly DbCommandInterceptor _dbCommandInterceptor = new DbCommandInterceptor();

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(DiagnosticListener listener)
        {
            if (listener.Name == DbLoggerCategory.Name)
            {
                listener.Subscribe(_dbCommandInterceptor);
            }
        }
    }
}