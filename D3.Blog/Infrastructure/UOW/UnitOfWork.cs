using System;
using System.Data;
using System.Threading.Tasks;
using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.Infrastructure;
using Infrastructure.Data.Database;
using Infrastructure.Identity.Data;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace Infrastructure.Data.UOW
{
    /// <summary>
    /// 工作单元实现
    /// </summary>
    public class UnitOfWork: IUnitOfWork
    {
        /// <summary>
        /// DB上下文对象
        /// </summary>
        private readonly D3BlogDbContext _dbContext;
        

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="baseRepository"></param>
        public UnitOfWork (D3BlogDbContext dbContext)
        {
            _dbContext  = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        
        public bool Commit()
        {
            return _dbContext.SaveChanges() > 0;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
