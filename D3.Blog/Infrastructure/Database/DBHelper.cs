using System;
using System.Data;
using System.Diagnostics;
using D3.Blog.Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace Infrastructure.Data.Database
{
    public class DBHelper : IDBHelper
    {
        private readonly D3BlogDbContext _dbContext;
        public DBHelper(D3BlogDbContext dbContext)
        {
            _dbContext  = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        /// <summary>
        /// 以ADO.NET方式执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet ExecSqlCommand(string sql)
        {
            using (var connection = (MySqlConnection) _dbContext.Database.GetDbConnection())
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                MySqlDataAdapter adapter=new MySqlDataAdapter(command);
                var ds=new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procNameSql">存储过程语句</param>
        /// <returns></returns>
        public DataSet ExecSqlStoredProcedure(string procNameSql)
        {
            using ( var connection = (MySqlConnection) _dbContext.Database.GetDbConnection())
            {
                if (connection.State!=ConnectionState.Open)
                {
                    connection.Open();
                }
                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = procNameSql;
                MySqlDataAdapter adapter=new MySqlDataAdapter(command);
                var ds=new DataSet();
                adapter.Fill(ds,"sumtime");
                connection.Close();
                return ds;
            }
            
        }


    }
}