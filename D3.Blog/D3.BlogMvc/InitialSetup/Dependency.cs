using D3.Blog.Domain.Infrastructure;
using Infrastructure.Data.Repository;
using Infrastructure.Data.UOW;
using MediatR;
using System.Reflection;
using D3.Blog.Application.AutoMapper;
using AutoMapper;
using D3.Blog.Application.Infrastructure;
using D3.Blog.Application.Interface;
using D3.Blog.Application.Services;
using D3.Blog.Application.Services.Articles;
using D3.Blog.Application.Services.Customer;
using D3.Blog.Domain.CommandHandlers.Articles;
using D3.Blog.Domain.CommandHandlers.Customer;
using D3.Blog.Domain.Commands.Articles;
using D3.Blog.Domain.Commands.Customer;
using D3.Blog.Domain.Core.BUS;
using D3.Blog.Domain.Core.Events;
using D3.Blog.Domain.Core.Notifications;
using D3.Blog.Domain.EventHandlers;
using D3.Blog.Domain.EventHandlers.ArticleEventHandler;
using D3.Blog.Domain.Events;
using D3.Blog.Domain.Events.ArticleEvent;
using D3.Blog.Domain.Infrastructure.IRepositorys;
using Infrastructure.Data.Bus;
using Infrastructure.Data.Database;
using Infrastructure.Data.EventSourcing;
using Infrastructure.Data.Repository.EventSourcing;
using Infrastructure.Data.Repository.Repositorys;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Http;

namespace D3.BlogMvc.InitialSetup
{
    /// <summary>
    /// 依赖注入
    /// </summary>
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

            // ASP.NET HttpContext dependency
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

            #region 单元工作与总线bus
            services.AddScoped(typeof(IUnitOfWork),typeof(UnitOfWork));
            services.AddScoped<IMediatorHandler,InMemoryBus>();
            #endregion


            services.AddMediatR();

            #region  Domain - Commands
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddScoped<IRequestHandler<RegisterNewCustomerCommand>, CustomerCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateCustomerCommand>, CustomerCommandHandler>();
            services.AddScoped<IRequestHandler<AddNewArticleCommand>, ArticleCommandHandle>();
            #endregion


            #region Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<INotificationHandler<CustomerRegisteredEvent>, CustomerEventHandler>();
            services.AddScoped<INotificationHandler<CustomerUpdatedEvent>, CustomerEventHandler>();
            services.AddScoped<INotificationHandler<ArticleAddOrEditEvent>, ArticleEventHandler>();
            #endregion
            

            #region 事件存储
            services.AddScoped<IEventStoreRepository, EventStoreSqlRepository>();
            services.AddScoped<IEventStore, SqlEventStore>();
            services.AddScoped<EventStoreSQLContext>();
            #endregion


            #region 仓储和服务
//            services.AddScoped(typeof(IRepository<>),typeof(Repository<>));
            services.AddScoped<ICustomerRepository,CustomerRepository>();
            services.AddScoped<IArticleRepository,ArticleRepository>();

            services.AddScoped<ICustomerService,CustomerService>();
            services.AddScoped<IArticleService,ArticleService>();
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
