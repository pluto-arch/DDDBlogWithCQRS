using System.Data;
using System.Threading.Tasks;

namespace Infrastructure.Data.Database
{
    public interface IDBHelper
    {
        /// <summary>
        /// 以ado.net方式执行sql语句
        /// </summary>
        /// <returns></returns>
        DataSet ExecSqlCommand(string sql);
        /// <summary>
        /// 以ADO.net方式执行存储过程
        /// </summary>
        /// <returns></returns>
        DataSet ExecSqlStoredProcedure(string procNameSql);
    }
}