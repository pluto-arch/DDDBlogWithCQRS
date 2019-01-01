using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.Infrastructure.IRepositorys;
using Infrastructure.Data.Database;

namespace Infrastructure.Data.Repository.Repositorys
{
    public class ArticleRepository: Repository<Article>, IArticleRepository
    {
        public ArticleRepository(D3BlogDbContext context) : base(context)
        {
        }

    }
}