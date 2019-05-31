using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.UOW
{
    public static class UnitOfWorkServiceCollectionExtensions
    {
        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services) where TContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            return services;
        }
        public static IServiceCollection AddCustomRepository<TEntity, TRepository>(this IServiceCollection services)
            where TEntity : BaseEntity
            where TRepository : class, IRepository<TEntity>
        {
            services.AddScoped<IRepository<TEntity>, TRepository>();

            return services;
        }
    }
}