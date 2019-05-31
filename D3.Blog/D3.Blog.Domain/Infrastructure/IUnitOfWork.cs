using System;
using System.Threading.Tasks;
using D3.Blog.Domain.Entitys;

namespace D3.Blog.Domain.Infrastructure
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public interface IUnitOfWork:IDisposable
    {
        /// <summary>
        /// 提交数据库
        /// </summary>
        /// <returns></returns>
        bool Commit();

        /// <summary>
        /// 获取对应仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);

    }
}
