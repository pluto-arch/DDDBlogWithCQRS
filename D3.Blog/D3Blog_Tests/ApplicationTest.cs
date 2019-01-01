using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using D3.Blog.Application.AutoMapper;
using D3.Blog.Domain.CommandHandlers.Customer;
using D3.Blog.Domain.Commands.Customer;
using D3.Blog.Domain.Core.BUS;
using D3.Blog.Domain.Core.Events;
using D3.Blog.Domain.Core.Notifications;
using D3.Blog.Domain.EventHandlers;
using D3.Blog.Domain.Events;
using D3.Blog.Domain.Infrastructure;
using D3.Blog.Domain.Infrastructure.IRepositorys;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Infrastructure.Identity.Data;
using Infrastructure.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Infrastructure.Data.Bus;
using Infrastructure.Data.Database;
using Infrastructure.Data.EventSourcing;
using Infrastructure.Data.Repository;
using Infrastructure.Data.Repository.EventSourcing;
using Infrastructure.Data.Repository.Repositorys;
using Infrastructure.Data.UOW;
using D3.Blog.Application.Infrastructure;
using D3.Blog.Application.Interface;
using D3.Blog.Application.Services;
using D3.Blog.Application.Services.Customer;
using D3.Blog.Application.ViewModels;
using D3.Blog.Domain.CommandHandlers.Articles;
using D3.Blog.Domain.Commands.Articles;
using D3.Blog.Domain.Entitys;
using Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace D3Blog_Tests
{
    public class ApplicationTest
    {
        /// <summary>
        /// EF �������ԣ��ֹ�����
        /// </summary>
        [Fact]
        public void ConcurrentHandle()
        {
            var res = 0;
            var com = new D3BlogDbContext();
            var com2 = new D3BlogDbContext();
            List<string> error = new List<string>();
            var customer = com.Customers.Find(1);
            com.Entry(customer).Property(x => x.BirthDate).CurrentValue = DateTime.Parse("2018-02-05");


            var cc = com2.Customers.FirstOrDefault();
            com2.Entry(cc).Property(x => x.Email).CurrentValue = "ccc@sina.com";
            com2.SaveChanges();

            try
            {
                res=  com.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                var exceptionEntry = e.Entries.Single();
                var clientValues = (Customer)exceptionEntry.Entity;//�ͻ���ֵ
                var databaseEntry = exceptionEntry.GetDatabaseValues();//���ݿ�ֵ
                if (databaseEntry == null)
                {
                    error.Add("�޷�������ġ��ò����ѱ������û�ɾ����");
                }
                else
                {
                    var databaseValues = (Customer)databaseEntry.ToObject();
                    //�ȽϿͻ���ֵ�����ݿ�ֵ����ʾ�û�
                    if (databaseValues.Name != clientValues.Name)
                    {
                        error.Add($"Current value: {databaseValues.Name}");
                    }
                    if (databaseValues.BirthDate != clientValues.BirthDate)
                    {
                        error.Add($"Current value: {databaseValues.BirthDate}");
                    }
                    if (databaseValues.Email != clientValues.Email)
                    {
                        error.Add($"Current value: {databaseValues.Email}");
                    }

                    com.Entry(customer).Property(x => x.RowVersion).OriginalValue = (byte[])databaseValues.RowVersion;//���¸������ƣ��û�����SaveChanges
                    com.Entry(customer).Property(x => x.Email).CurrentValue = "11111@qq.com";
                    res= com.SaveChanges();
                }
            }
            
            Assert.True(res>0);
        }


        /// <summary>
        /// ����Ԥ�������
        /// </summary>
        [Fact]
        public async Task PreRequestTest()
        {
            IServiceProvider serviceProvider = BuildService();
            var customerservice = serviceProvider.GetRequiredService<ICustomerService>();//��ȡʵ��
            var customer = new CustomerViewModel
            {
                Name = "abc",
                BirthDate = DateTime.Parse("1994-02-05"),
                Email = "123123123@qq.com"
            };
            customerservice.Add(customer);

            var c= await customerservice.GetById(1);
            if (c!=null)
            {
                try
                {
                    c.BirthDate = DateTime.Parse("2018-01-03");
                    customerservice.Update(c);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
               
            }

            Assert.Equal(0,customer.Id);
        }
        /// <summary>
        /// ������²���
        /// </summary>
        [Fact]
        public void ArticleAddTest()
        {
            IServiceProvider serviceProvider = BuildService();
            var bus = serviceProvider.GetRequiredService<IMediatorHandler>();//��ȡʵ��
            var articleRep = serviceProvider.GetRequiredService<IArticleRepository>();//��ȡʵ��
            //AddNewArticleCommand
            var addcmd = new AddNewArticleCommand("asp","<p>this is asp</p>","������",1,"�Դ�","asp","asp","asp","../../1.jpg");//��ȡʵ��

            bus.SendCommand(addcmd).Wait();
            var articles=articleRep.FindAll().Include(x=>x.ArticleCategory).ToList();

            Assert.True(articles.Count>0);
        }


        /// <summary>
        /// ע������
        /// </summary>
        /// <returns></returns>
        public IServiceProvider BuildService()
        {
            var services = new ServiceCollection();

            #region ע��
             services.ConfigureSeriLog(GetConfiguration());//����serilog
            // ASP.NET HttpContext dependency
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();
            #region ��Ԫ����������bus
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
            #endregion
            #region �¼��洢
            services.AddScoped<IEventStoreRepository, EventStoreSqlRepository>();
            services.AddScoped<IEventStore, SqlEventStore>();
            services.AddScoped<EventStoreSQLContext>();
            #endregion

            services.AddAutoMapper();

            // Registering Mappings automatically only works if the 
            // Automapper Profile classes are in ASP.NET project
            AutoMapperConfig.RegisterMappings();

            #region �ִ��ͷ���
            services.AddScoped(typeof(IRepository<>),typeof(Repository<>));
            services.AddScoped<ICustomerRepository,CustomerRepository>();
            services.AddScoped<ICustomerService,CustomerService>();
            services.AddScoped<IArticleRepository,ArticleRepository>();
            #endregion


            #region �¼��洢��Ҫ
            services.AddScoped(typeof(AppBlogUser));
            #endregion

            #region BlogDbContext
            services.AddScoped<D3BlogDbContext>();
            #endregion
            

            #endregion

            IServiceProvider ser=services.BuildServiceProvider();

            return ser; //���������ṩ����
        }
        public IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            return builder.Build();
        }

    }
}
