using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
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
using D3.Blog.Application.Services.Articles;
using D3.Blog.Application.Services.Customer;
using D3.Blog.Application.Services.PostGroup;
using D3.Blog.Application.ViewModels;
using D3.Blog.Domain.CommandHandlers.Articles;
using D3.Blog.Domain.CommandHandlers.PostGroup;
using D3.Blog.Domain.Commands.Articles;
using D3.Blog.Domain.Commands.PostGroup;
using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.EventHandlers.PostGroupEventHandler;
using D3.Blog.Domain.Events.ArticleEvent;
using D3.Blog.Domain.Events.PostGroup;
using Infrastructure.AOP;
using Infrastructure.Logging;
using Infrastructure.Tools;
using Microsoft.Extensions.DependencyInjection;
using DependencyInjection;
using MySql.Data.MySqlClient;
using Xunit.Abstractions;

namespace D3Blog_Tests
{
    public class ApplicationTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ApplicationTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        /// <summary>
        /// EF 并发测试（乐观锁）
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
                var clientValues = (Customer)exceptionEntry.Entity;//客户端值
                var databaseEntry = exceptionEntry.GetDatabaseValues();//数据库值
                if (databaseEntry == null)
                {
                    error.Add("无法保存更改。该部门已被其他用户删除。");
                }
                else
                {
                    var databaseValues = (Customer)databaseEntry.ToObject();
                    //比较客户端值和数据库值，提示用户
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

                    com.Entry(customer).Property(x => x.RowVersion).OriginalValue = (byte[])databaseValues.RowVersion;//重新赋予令牌，用户可以SaveChanges
                    com.Entry(customer).Property(x => x.Email).CurrentValue = "11111@qq.com";
                    res= com.SaveChanges();
                }
            }
            
            Assert.True(res>0);
        }


        /// <summary>
        /// 请求预处理测试
        /// </summary>
        [Fact]
        public async Task PreRequestTest()
        {
            IServiceProvider serviceProvider = BuildService();
            var customerservice = serviceProvider.GetRequiredService<ICustomerService>();//获取实例
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
        /// 添加文章测试
        /// </summary>
        [Fact]
        public void ArticleAddTest()
        {
            IServiceProvider serviceProvider = BuildService();
            var bus = serviceProvider.GetRequiredService<IMediatorHandler>();//获取实例
            var articleRep = serviceProvider.GetRequiredService<IArticleRepository>();//获取实例
            var articles=articleRep.FindAll().Include(x=>x.ArticleCategory).ToList();

            Assert.True(articles.Count>0);
        }

        /// <summary>
        /// 添加文章分组测试
        /// </summary>
        [Fact]
        public void PostGroupTets()
        {
            IServiceProvider serviceProvider = BuildService();
            var bus = serviceProvider.GetRequiredService<IMediatorHandler>();//获取实例
            var _notifications = (DomainNotificationHandler)serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();
            var addcmd = new AddPostGroupCommands("IDS系列", 2);

            bus.SendCommand(addcmd);
            var error = _notifications.GetNotifications().Select(n => n.Value);//通知结果
            Assert.True(true);

        }



        /// <summary>
        /// 添加文章分组测试
        /// </summary>
        [Fact]
        public void BulkToDB()
        {
            DataTable table = new DataTable();
            table.TableName = "PostSeries";
            table.Columns.AddRange(new DataColumn[] {
                                new DataColumn("GroupName",typeof(string)),
                                new DataColumn("OwinUserId",typeof(string))
                             });
            for (int i = 0; i < 2000000; i++)
            {
                DataRow dr = table.NewRow();
                dr["GroupName"] = i;
                dr["OwinUserId"] = i++;
                table.Rows.Add(dr);
            }

            Stopwatch sw = new Stopwatch();
            MySqlConnection sqlconn = new MySqlConnection("Server=35.240.152.58; Port=3306; Database=zylblog; Uid=root; Pwd=970307Lbx$;");
            ToCsv(table);
            using (sqlconn)
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + table.TableName + ".csv";
                var columns = table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList();
                Stopwatch stopwatch = new Stopwatch(); stopwatch.Start();
                sqlconn.Open();
                MySqlBulkLoader bulk = new MySqlBulkLoader(sqlconn)
                {
                    FieldTerminator = ",",
                    FieldQuotationCharacter = '"',
                    EscapeCharacter = '"',
                    LineTerminator = "\r\n",
                    FileName = AppDomain.CurrentDomain.BaseDirectory + table.TableName+ ".csv",
                    NumberOfLinesToSkip = 0,
                    TableName = table.TableName,
                };

                bulk.Columns.AddRange(columns);
                var aaa= bulk.Load();

                stopwatch.Stop();
                _testOutputHelper.WriteLine("耗时:{0}", stopwatch.ElapsedMilliseconds);
                Assert.True(stopwatch.ElapsedMilliseconds>0);
            }

        }


        public void ToCsv(DataTable table)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }

            File.WriteAllText(table.TableName + ".csv", sb.ToString());
        }









        /// <summary>
        /// 注入容器
        /// </summary>
        /// <returns></returns>
        public IServiceProvider BuildService()
        {
            var services = new ServiceCollection();
            DiagnosticListener.AllListeners.Subscribe(new CommandListener());


            services.ServerDependencies(); //配置依赖注入  
//            services.AddAutoMapperSetup();
            services.AddMemoryCache();
            services.AddMvc();

            #region Autofac  暂时当作service和repoitory的拦截器使用

            var builder = new ContainerBuilder();
            builder.Populate(services);//将原生的注入填充进去
            builder.RegisterType<BlogLogAOP>();//可以直接替换其他拦截器！一定要把拦截器进行注册
            builder.RegisterType<BlogCacheAOP>();
            builder.ServerDependenciesAutofac();
            var applicationContainer = builder.Build();//构建新容器

            #endregion

            return new AutofacServiceProvider(applicationContainer);//新容器
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
