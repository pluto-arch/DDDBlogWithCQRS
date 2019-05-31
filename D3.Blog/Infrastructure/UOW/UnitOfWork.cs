using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.Infrastructure;
using Infrastructure.Data.Database;
using Infrastructure.Data.Repository;
using Infrastructure.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Infrastructure.Data.UOW
{
    /// <summary>
    /// 工作单元实现
    /// </summary>
    public class UnitOfWork<TContext>: IUnitOfWork where TContext:DbContext
    {
        /// <summary>
        /// DB上下文对象
        /// </summary>
        private readonly TContext _dbContext;
        /// <summary>
        /// 仓储集合
        /// </summary>
        private Dictionary<Type, object> repositories;


        private IServiceProvider IocConfig;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="iocConfig"></param>
        public UnitOfWork (TContext dbContext, IServiceProvider iocConfig)
        {
            _dbContext  = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            IocConfig = iocConfig;
        }

        /// <summary>
        /// 数据库上下文
        /// </summary>
        public TContext DbContext => _dbContext;

        /// <summary>
        /// 提交数据库
        /// </summary>
        /// <returns></returns>
        public bool Commit()
        {
            return _dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 获取对应仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }
            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = IocConfig.GetService(typeof(IRepository<TEntity>)); 
            }

            return (IRepository<TEntity>)repositories[type];
        }

        /// <summary>
        /// 直接执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return _dbContext.Database.ExecuteSqlCommand(sql, parameters);
        }

        public void Dispose()
        {
            // clear repositories
            if (repositories != null)
            {
                repositories.Clear();
            }

            _dbContext.Dispose();
        }
    }
}
