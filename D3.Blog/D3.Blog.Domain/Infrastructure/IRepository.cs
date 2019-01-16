using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using D3.Blog.Domain.Entitys;

namespace D3.Blog.Domain.Infrastructure
{
    public interface IRepository<T> where T:BaseEntity
    {
        /// <summary>
        /// 根据主值查询单条数据
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <returns>泛型实体</returns>
        Task<T> FindByIdAsync(object pkValue);

        /// <summary>
        /// 查询所有数据(无分页,请慎用)
        /// </summary>
        /// <returns></returns>
        IQueryable<T> FindAll();



        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderBy">排序</param>
        /// <returns>泛型实体集合</returns>
        IQueryable<T> FindListByClause<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderby);

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <returns>一个T的实体</returns>
        T FindByClause(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>新增的T的主键值</returns>
        void Insert(T entity);

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>true：false</returns>
        void Update(T entity);

        /// <summary>
        /// 删除数据(按整个实体)
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>true:false</returns>
        void Delete(T entity);
        /// <summary>
        /// 删除数据（按条件）
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        void Delete(Expression<Func<T, bool>> @where);

        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void DeleteById(object id);

        /// <summary>
        /// 删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        void DeleteByIds(object[] ids);
    }
}
