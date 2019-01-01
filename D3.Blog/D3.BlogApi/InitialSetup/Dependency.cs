using Infrastructure.Data.UOW;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using D3.Blog.Application.Interface;
using D3.Blog.Application.Services;
using D3.Blog.Application.Services.Customer;
using D3.Blog.Domain.CommandHandlers.Customer;
using D3.Blog.Domain.Commands.Customer;
using D3.Blog.Domain.Core.BUS;
using D3.Blog.Domain.Core.Events;
using D3.Blog.Domain.Core.Notifications;
using D3.Blog.Domain.EventHandlers;
using D3.Blog.Domain.Events;
using D3.Blog.Domain.Infrastructure;
using D3.Blog.Domain.Infrastructure.IRepositorys;
using D3.BlogApi.AuthHelper;
using Infrastructure.Data.Bus;
using Infrastructure.Data.Database;
using Infrastructure.Data.EventSourcing;
using Infrastructure.Data.Repository;
using Infrastructure.Data.Repository.EventSourcing;
using Infrastructure.Data.Repository.Repositorys;
using Infrastructure.Identity.Models;

namespace D3.BlogApi.InitialSetup
{
    /// <summary>
    /// 依赖注入
    /// </summary>d
    internal static class Dependency
    {
        /// <summary>
        /// 配置依赖注入
        /// IServiceCollection：负责注册
        /// IServiceProvider：负责提供实例
        /// </summary>
        /// <param name="services"></param>
        internal static void ConfigureDependencies(this IServiceCollection services)
        {
        #region 但愿工作与总线bus
            services.AddScoped(typeof(IUnitOfWork),typeof(UnitOfWork));
            services.AddScoped<IMediatorHandler,InMemoryBus>();
        #endregion

            services.AddScoped(typeof(JwtHelper));
            services.AddMediatR();//注入中介者
            

        #region  Domain - Commands
            services.AddScoped<IRequestHandler<RegisterNewCustomerCommand>, CustomerCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateCustomerCommand>, CustomerCommandHandler>();
        #endregion


        #region Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<INotificationHandler<CustomerRegisteredEvent>, CustomerEventHandler>();
            services.AddScoped<INotificationHandler<CustomerUpdatedEvent>, CustomerEventHandler>();
        #endregion
            

        #region 事件存储
            services.AddScoped<IEventStoreRepository, EventStoreSqlRepository>();
            services.AddScoped<IEventStore, SqlEventStore>();
            services.AddScoped<EventStoreSQLContext>();
        #endregion


        #region 仓储和服务
            services.AddScoped(typeof(IRepository<>),typeof(Repository<>));
            services.AddScoped<ICustomerRepository,CustomerRepository>();
            services.AddScoped<ICustomerService,CustomerService>();
        #endregion


        #region 事件存储需要
            services.AddScoped(typeof(AppBlogUser));
        #endregion
            #region BlogDbContext
            services.AddScoped<D3BlogDbContext>();
            #endregion
        }
    }
}
