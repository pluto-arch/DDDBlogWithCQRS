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
//        internal readonly Serilog.ILogger _logger;

        public ArticleRepository(D3BlogDbContext context,Serilog.ILogger logger)
        {
            _context = context;
            _dbSet   = context.Set<Article>();
//            _logger = logger;
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
            try
            {
                return  _dbSet.FirstAsync(x => x.Id == int.Parse(pkValue.ToString()));
            }
            catch (Exception e)
            {
//                _logger.Information(e,$"出现错误：{e.Message}");
            }
            return null;
        }

        public override IQueryable<Article> FindListByClause<TKey>(Expression<Func<Article, bool>> predicate, Expression<Func<Article, TKey>> orderby)
        {
            return _dbSet.Where(predicate).OrderByDescending(orderby).AsQueryable();
        }


        public override IQueryable<Article> FindListByPage<TKey>(int pageSize,int pageIndex,Expression<Func<Article, bool>> predicate, Expression<Func<Article, TKey>> orderby)
        {
            var result = _dbSet.Where(predicate).OrderByDescending(orderby).Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
            return result.AsQueryable();
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