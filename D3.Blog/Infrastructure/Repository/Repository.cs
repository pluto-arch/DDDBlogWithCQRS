using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.Infrastructure;
using Infrastructure.Data.Database;
using Infrastructure.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repository
{
    public  class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking; //更改上下文级别上的默认跟踪行为
            _dbSet = _dbContext.Set<TEntity>();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Insert(TEntity entity)
        {
            var entry = _dbSet.Add(entity);
        }

        /// <summary>
        /// 同步插入一系列实体 1
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Insert(params TEntity[] entities)
        {
            _dbSet.AddRange(entities);
        }
        /// <summary>
        /// 同步插入一系列实体 2
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> FromSql(string sql, params object[] parameters)
        {
           return _dbSet.FromSql(sql, parameters);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }


        public virtual void DeleteById(object id)
        {
            // 使用存根实体标记删除
            var typeInfo = typeof(TEntity).GetTypeInfo();
            var key = _dbContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
            var property = typeInfo.GetProperty(key?.Name);
            if (property != null)
            {
                var entity = Activator.CreateInstance<TEntity>();
                property.SetValue(entity, id);
                _dbContext.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                var entity = _dbSet.Find(id);
                if (entity != null)
                {
                    Delete(entity);
                }
            }
        }

        public virtual void Delete(params TEntity[] entities) => _dbSet.RemoveRange(entities);



        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {

        }

        public virtual void DeleteByIds(object[] ids)
        {

        }





        //----------------------------------------------------
        /// <summary>
        /// 查询全部  尽量少用  建议使用page
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> FindAll()
        {
            return _dbSet;
        }

        /// <summary>
        /// 按条件查询一个
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual TEntity FindByClause(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        /// <summary>
        /// 按id查询
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public virtual Task<TEntity> FindByIdAsync(params object[] keyValues) => _dbSet.FindAsync(keyValues);

        /// <summary>
        /// 通过id查询  同步
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public virtual TEntity FindById(params object[] keyValues) => _dbSet.Find(keyValues);



        /// <summary>
        /// 按条件查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="orderby"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> FindListByClause<TKey>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TKey>> orderby,bool isAsc)
        {
            if (isAsc)
            {
                return _dbSet.Where(predicate).OrderBy(orderby).AsQueryable();
            }
            return _dbSet.Where(predicate).OrderByDescending(orderby).AsQueryable();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageSiza"></param>
        /// <param name="pageIndex"></param>
        /// <param name="predicate"></param>
        /// <param name="orderby"></param>
        /// <param name="isAsc"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> FindListByPage<TKey>(int pageSiza, int pageIndex,
            Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderby, bool isAsc, out int count)
        {
            if (isAsc)
            {
                var result = _dbSet.Where(predicate).OrderBy(orderby).Skip((pageIndex - 1) * pageSiza)
                    .Take(pageSiza).AsNoTracking();  //查询不启用跟踪，优化性能
                count = _dbSet.Count();
                return result.AsQueryable();
            }
            else
            {
                var result = _dbSet.Where(predicate).OrderByDescending(orderby).Skip((pageIndex - 1) * pageSiza)
                    .Take(pageSiza).AsNoTracking();
                count = _dbSet.Count();
                return result.AsQueryable();
            }
        }

    }
}
