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
        public ArticleRepository(D3BlogDbContext context):base(context)
        { }

        public override void Update(Article entity)
        {
            _dbSet.Attach(entity).Property(x=>x.Status).IsModified=true; //EF Core开始跟踪entity实体的更改并告诉告诉EF Core实体entity的Name属性已经更改 
        }
    }
}