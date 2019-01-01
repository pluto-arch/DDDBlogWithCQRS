using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.Infrastructure;
using Infrastructure.Data.Database;
using Infrastructure.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    { 
        internal   D3BlogDbContext _context;
        internal DbSet<TEntity>       _dbSet;

        public Repository(D3BlogDbContext context)
        {
            this._context = context;
            this._dbSet   = context.Set<TEntity>();
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Attach(entity);//告诉EF Core开始跟踪TEntity实体的更改
            _context.Entry(entity).State = EntityState.Modified;//告诉EF 整个TEntity实体都被修改了(默认)。会出现并发问题。可通过重写解决
        }
        public virtual void Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }


        public virtual void DeleteById(object id)
        {
            Delete(_dbSet.Find(id));
        }
        
        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteByIds(object[] ids)
        {
            throw new NotImplementedException();
        }
        //----------------------------------------------------
        public virtual IQueryable<TEntity> FindAll()
        {
            return _dbSet;
        }

        public virtual TEntity FindByClause(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TEntity> FindByIdAsync(object pkValue)
        {
            return await _dbSet.FindAsync(pkValue);
        }

        public virtual IQueryable<TEntity> FindListByClause(Expression<Func<TEntity, bool>> predicate, string orderBy)
        {
            throw new NotImplementedException();
        }

    }
}
