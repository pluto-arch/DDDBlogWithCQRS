using D3.Blog.Domain.Infrastructure;
using Infrastructure.Data.Repository;
using Infrastructure.Data.UOW;
using MediatR;
using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
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
using Infrastructure.AOP;
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
        /// 自带配置依赖注入
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
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped<IMediatorHandler, InMemoryBus>();
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
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();

            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IArticleService, ArticleService>();
            #endregion

            #region 事件存储需要
            services.AddScoped(typeof(AppBlogUser));
            #endregion

            #region BlogDbContext
            services.AddScoped<D3BlogDbContext>();
            #endregion

        }


        /// <summary>
        /// autofac 注入
        /// </summary>
        /// <param name="builder"></param>
        internal static void ConfigureDependenciesAutofac(this ContainerBuilder builder)
        {
            
            builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();
            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterType<HttpContextAccessor>().As(typeof(IHttpContextAccessor)).SingleInstance();
            builder.RegisterType<AspNetUser>().As(typeof(IUser)).InstancePerLifetimeScope();



            #region 单元工作与总线bus
            builder.RegisterType<UnitOfWork>().As(typeof(IUnitOfWork)).InstancePerLifetimeScope();
            builder.RegisterType<InMemoryBus>().As(typeof(IMediatorHandler)).InstancePerLifetimeScope();

            #endregion



            #region  Domain - Commands
            builder.RegisterGeneric(typeof(RequestPerformanceBehaviour<,>)).As(typeof(IPipelineBehavior<,>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(RequestValidationBehavior<,>)).As(typeof(IPipelineBehavior<,>)).InstancePerDependency();

            builder.RegisterType<CustomerCommandHandler>().As(typeof(IRequestHandler<RegisterNewCustomerCommand>)).InstancePerLifetimeScope();
            builder.RegisterType<CustomerCommandHandler>().As(typeof(IRequestHandler<UpdateCustomerCommand>)).InstancePerLifetimeScope();
            builder.RegisterType<ArticleCommandHandle>().As(typeof(IRequestHandler<AddNewArticleCommand>)).InstancePerLifetimeScope();
            #endregion


            #region Domain - Events
            builder.RegisterType<DomainNotificationHandler>().As(typeof(INotificationHandler<DomainNotification>)).InstancePerLifetimeScope();
            builder.RegisterType<CustomerEventHandler>().As(typeof(INotificationHandler<CustomerRegisteredEvent>)).InstancePerLifetimeScope();
            builder.RegisterType<CustomerEventHandler>().As(typeof(INotificationHandler<CustomerUpdatedEvent>)).InstancePerLifetimeScope();
            builder.RegisterType<ArticleEventHandler>().As(typeof(INotificationHandler<ArticleAddOrEditEvent>)).InstancePerLifetimeScope();

            #endregion


            #region 事件存储
            builder.RegisterType<EventStoreSqlRepository>().As(typeof(IEventStoreRepository)).InstancePerLifetimeScope();
            builder.RegisterType<SqlEventStore>().As(typeof(IEventStore)).InstancePerLifetimeScope();
            builder.RegisterType<EventStoreSQLContext>().InstancePerLifetimeScope();
            #endregion


            #region 仓储和服务
            builder.RegisterType<CustomerRepository>().As(typeof(ICustomerRepository)).InstancePerLifetimeScope();
            builder.RegisterType<ArticleRepository>().As(typeof(IArticleRepository)).InstancePerLifetimeScope();

            builder.RegisterType<CustomerService>().As(typeof(ICustomerService)).InstancePerLifetimeScope();
            builder.RegisterType<ArticleService>().As(typeof(IArticleService))
                .InstancePerLifetimeScope();


            /*
             *  builder.RegisterType<CustomerRepository>().As(typeof(ICustomerRepository)).InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()//启动动态代理，拦截器
                .InterceptedBy(typeof(BlogLogAOP));//附加拦截器;
            builder.RegisterType<ArticleRepository>().As(typeof(IArticleRepository)).InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()//启动动态代理，拦截器
                .InterceptedBy(typeof(BlogLogAOP));//附加拦截器;

            builder.RegisterType<CustomerService>().As(typeof(ICustomerService)).InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()//启动动态代理，拦截器
                .InterceptedBy(typeof(BlogLogAOP));//附加拦截器;
            builder.RegisterType<ArticleService>().As(typeof(IArticleService))
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()//启动动态代理，拦截器
                .InterceptedBy(typeof(BlogLogAOP))
                .InterceptedBy(typeof(BlogCacheAOP));//附加拦截器
             */


            #endregion

            #region 事件存储需要

            builder.RegisterType<AppBlogUser>().InstancePerLifetimeScope();
            #endregion

            #region BlogDbContext
            builder.RegisterType<D3BlogDbContext>().InstancePerLifetimeScope();
            #endregion

        }

    }
}
