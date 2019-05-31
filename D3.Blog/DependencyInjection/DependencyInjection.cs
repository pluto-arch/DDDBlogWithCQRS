using Autofac;
using Autofac.Extras.DynamicProxy;
using D3.Blog.Application.Infrastructure;
using D3.Blog.Application.Interface;
using D3.Blog.Application.Services.Articles;
using D3.Blog.Application.Services.Customer;
using D3.Blog.Application.Services.PostGroup;
using D3.Blog.Domain.CommandHandlers.Articles;
using D3.Blog.Domain.CommandHandlers.Customer;
using D3.Blog.Domain.CommandHandlers.PostGroup;
using D3.Blog.Domain.Commands.Articles;
using D3.Blog.Domain.Commands.Customer;
using D3.Blog.Domain.Commands.PostGroup;
using D3.Blog.Domain.Core.BUS;
using D3.Blog.Domain.Core.Events;
using D3.Blog.Domain.Core.Notifications;
using D3.Blog.Domain.EventHandlers;
using D3.Blog.Domain.EventHandlers.ArticleEventHandler;
using D3.Blog.Domain.EventHandlers.PostGroupEventHandler;
using D3.Blog.Domain.Events;
using D3.Blog.Domain.Events.ArticleEvent;
using D3.Blog.Domain.Events.PostGroup;
using D3.Blog.Domain.Infrastructure;
using D3.Blog.Domain.Infrastructure.IRepositorys;
using Infrastructure.AOP;
using Infrastructure.Data.Bus;
using Infrastructure.Data.Database;
using Infrastructure.Data.EventSourcing;
using Infrastructure.Data.Repository.EventSourcing;
using Infrastructure.Data.Repository.Repositorys;
using Infrastructure.Data.UOW;
using Infrastructure.Identity.Models;
using Infrastructure.NLoger;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using D3.Blog.Domain.Entitys;
using System.Reflection;
using D3.Blog.Domain.CommandHandlers;

namespace DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ServerDependencies(this IServiceCollection services,Type t)
        {
            // ASP.NET HttpContext dependency
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();
            services.AddScoped<ICustomerLogging, CustomerLogging>();
            services.AddScoped<IDBHelper, DBHelper>();
            services.AddMediatR(typeof(CommandHandler).GetTypeInfo().Assembly);



            #region 单元工作,仓储,总线bus
            services.AddScoped<D3BlogDbContext>().AddUnitOfWork<D3BlogDbContext>();
//            services.AddCustomRepository<Customer, CustomerRepository>();
//            services.AddCustomRepository<Article, ArticleRepository>();
//            services.AddCustomRepository<PostSeries, PostGroupRepository>();
            services.AddScoped<IMediatorHandler, InMemoryBus>();
            #endregion

            #region  Domain - Commands
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));//执行性能监视
            services.AddScoped<IRequestHandler<RegisterNewCustomerCommand>, CustomerCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateCustomerCommand>, CustomerCommandHandler>();
            services.AddScoped<IRequestHandler<AddNewArticleCommand>, ArticleCommandHandle>();
            services.AddScoped<IRequestHandler<AddPostGroupCommands>, PostGroupCommandHandler>();
            #endregion


            #region Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<INotificationHandler<CustomerRegisteredEvent>, CustomerEventHandler>();
            services.AddScoped<INotificationHandler<CustomerUpdatedEvent>, CustomerEventHandler>();
            services.AddScoped<INotificationHandler<ArticleAddOrEditEvent>, ArticleEventHandler>();
            services.AddScoped<INotificationHandler<PostGroupAddOrEditEvent>, PostGroupEventHandler>();
            #endregion


            #region 事件存储
            services.AddScoped<IEventStoreRepository, EventStoreSqlRepository>();
            services.AddScoped<IEventStore, SqlEventStore>();
            services.AddScoped<EventStoreSQLContext>();
            #endregion
         
            #region 事件存储需要
            services.AddScoped(typeof(AppBlogUser));
            #endregion

            
        }


        /// <summary>
        /// autofac 注入
        /// </summary>
        /// <param name="builder"></param>
        public static void ServerDependenciesAutofac(this ContainerBuilder builder)
        {

            builder.RegisterType<CustomerRepository>().As(typeof(IRepository<Customer>)).InstancePerLifetimeScope()
               .EnableInterfaceInterceptors()//启动动态代理，拦截器
               .InterceptedBy(typeof(BlogLogAOP));//附加拦截器;
            builder.RegisterType<ArticleRepository>().As(typeof(IRepository<Article>)).InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()//启动动态代理，拦截器
                .InterceptedBy(typeof(BlogLogAOP));//附加拦截器;
            builder.RegisterType<PostGroupRepository>().As(typeof(IRepository<PostSeries>)).InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()//启动动态代理，拦截器
                .InterceptedBy(typeof(BlogLogAOP));//附加拦截器;



            builder.RegisterType<CustomerService>().As(typeof(ICustomerService)).InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()//启动动态代理，拦截器
                .InterceptedBy(typeof(BlogLogAOP));//附加拦截器;
            builder.RegisterType<ArticleService>().As(typeof(IArticleService))
                .InstancePerLifetimeScope();
                //.EnableInterfaceInterceptors() //启动动态代理，拦截器
                //.InterceptedBy(typeof(BlogLogAOP));
            //              .InterceptedBy(typeof(BlogCacheAOP));//附加拦截器
            builder.RegisterType<PostGroupServer>().As(typeof(IPostGroupServer)).InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()//启动动态代理，拦截器
                .InterceptedBy(typeof(BlogLogAOP));//附加拦截器;
        }


        public static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                    optional: true);
            return builder.Build();
        }



    }
}
