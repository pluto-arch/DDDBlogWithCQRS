using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MySql.Data.MySqlClient;

namespace Infrastructure.Tools
{
    public class DbCommandInterceptor : IObserver<KeyValuePair<string, object>>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(KeyValuePair<string, object> value)
        {
            if (value.Key == RelationalEventId.CommandExecuting.Name)
            {
                var command = ((CommandEventData)value.Value).Command;
                var executeMethod = ((CommandEventData)value.Value).ExecuteMethod;
                GetDbCommand(command);
            }
        }

        /// <summary>
        /// 获取执行中的SQL语句
        /// </summary>
        /// <param name="command"></param>
        void GetDbCommand(DbCommand command)
        {
            var sql = command.CommandText;
            var parame = command.Parameters;
            
        }
    }

}