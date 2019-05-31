using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.Infrastructure.IRepositorys;
using Infrastructure.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repository.Repositorys
{
    public class PostGroupRepository : Repository<PostSeries>, IPostGroupRepository
    {
        public PostGroupRepository(D3BlogDbContext context):base(context)
        {}

    }
}