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
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        public abstract void Insert(TEntity entity);

        public abstract void Update(TEntity entity);
//        {
//            _dbSet.Attach(entity);//告诉EF Core开始跟踪TEntity实体的更改
//            _context.Entry(entity).State = EntityState.Modified;//告诉EF 整个TEntity实体都被修改了(默认)。会出现并发问题。可通过重写解决
//        }

        public abstract void Delete(TEntity entity);


        public abstract void DeleteById(object id);

        public abstract void Delete(Expression<Func<TEntity, bool>> where);

        public abstract void DeleteByIds(object[] ids);
        //----------------------------------------------------
        public abstract IQueryable<TEntity> FindAll();

        public abstract TEntity FindByClause(Expression<Func<TEntity, bool>> predicate);

        public abstract Task<TEntity> FindByIdAsync(object pkValue);

        public abstract IQueryable<TEntity> FindListByClause<TKey>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TKey>> orderby);

    }
}
