using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.Infrastructure.IRepositorys;
using Infrastructure.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repository.Repositorys
{
    public class PostGroupRepository : IPostGroupRepository
    {
        internal D3BlogDbContext _context;
        internal DbSet<PostSeries> _dbSet;

        public PostGroupRepository(D3BlogDbContext context)
        {
            _context = context;
            _dbSet = context.Set<PostSeries>();
        }

        public void Delete(PostSeries entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<PostSeries, bool>> where)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(object id)
        {
            throw new NotImplementedException();
        }

        public void DeleteByIds(object[] ids)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PostSeries> FindAll()
        {
            throw new NotImplementedException();
        }

        public PostSeries FindByClause(Expression<Func<PostSeries, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<PostSeries> FindByIdAsync(object pkValue)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PostSeries> FindListByClause<TKey>(Expression<Func<PostSeries, bool>> predicate, Expression<Func<PostSeries, TKey>> orderby)
        {
            var result = _dbSet.Where(predicate).OrderBy(orderby);
            return result;
        }

        public IQueryable<PostSeries> FindListByPage<TKey>(int pageSiza, int pageIndex, Expression<Func<PostSeries, bool>> predicate, Expression<Func<PostSeries, TKey>> orderby)
        {
            throw new NotImplementedException();
        }

        public void Insert(PostSeries entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(PostSeries entity)
        {
            throw new NotImplementedException();
        }
    }
}