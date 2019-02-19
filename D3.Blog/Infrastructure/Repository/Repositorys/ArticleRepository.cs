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
    /// <summary>
    /// 文章仓储
    /// </summary>
    public class ArticleRepository: Repository<Article>, IArticleRepository
    {
        internal   D3BlogDbContext _context;
        internal DbSet<Article>       _dbSet;

        public ArticleRepository(D3BlogDbContext context)
        {
            _context = context;
            _dbSet   = context.Set<Article>();
        }

        public override void Delete(Article entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(Expression<Func<Article, bool>> where)
        {
            throw new NotImplementedException();
        }

        public override void DeleteById(object id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteByIds(object[] ids)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<Article> FindAll()
        {
            throw new NotImplementedException();
        }

        public override Article FindByClause(Expression<Func<Article, bool>> predicate)
        {
            return _dbSet.First(predicate);
        }

        public override Task<Article> FindByIdAsync(object pkValue)
        {
            return  _dbSet.FirstAsync(x => x.Id == int.Parse(pkValue.ToString()));
        }

        public override IQueryable<Article> FindListByClause<TKey>(Expression<Func<Article, bool>> predicate, Expression<Func<Article, TKey>> orderby)
        {
            return _dbSet.Where(predicate).OrderByDescending(orderby).AsQueryable();
        }


        public override IQueryable<Article> FindListByPage<TKey>(int pageSize,int pageIndex,Expression<Func<Article, bool>> predicate, Expression<Func<Article, TKey>> orderby)
        {
            if (predicate!=null)
            {
                var result = _dbSet.Where(predicate).OrderByDescending(orderby).Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize);
                return result.AsQueryable();
            }
            else
            {
                var result = _dbSet.OrderByDescending(orderby).Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize);
                return result.AsQueryable();
            }
           
            
        }


        public override void Insert(Article entity)
        {
            _dbSet.Add(entity);
        }


        public override void Update(Article entity)
        {
            throw new NotImplementedException();
        }
    }
}