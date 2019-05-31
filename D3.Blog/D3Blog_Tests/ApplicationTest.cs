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
            var articles=articleRep.FindAll().Include(x=>x.ArticleCategory).ToList();

            Assert.True(articles.Count>0);
        }

        /// <summary>
        /// ������·������
        /// </summary>
        [Fact]
        public void PostGroupTets()
        {
            IServiceProvider serviceProvider = BuildService();
            var bus = serviceProvider.GetRequiredService<IMediatorHandler>();//��ȡʵ��
            var _notifications = (DomainNotificationHandler)serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();
            var addcmd = new AddPostGroupCommands("IDSϵ��", 2);

            bus.SendCommand(addcmd);
            var error = _notifications.GetNotifications().Select(n => n.Value);//֪ͨ���
            Assert.True(true);

        }



        /// <summary>
        /// ������·������
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
                _testOutputHelper.WriteLine("��ʱ:{0}", stopwatch.ElapsedMilliseconds);
                Assert.True(stopwatch.ElapsedMilliseconds>0);
            }

        }


        public void ToCsv(DataTable table)
        {
            //�԰�Ƕ��ţ���,�����ָ�������Ϊ��ҲҪ�������ڡ�
            //����������ڰ�Ƕ��ţ���,�����ð�����ţ���""�������ֶ�ֵ����������
            //����������ڰ�����ţ���"����Ӧ�滻�ɰ��˫���ţ�""��ת�壬���ð�����ţ���""�������ֶ�ֵ����������
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
        /// ע������
        /// </summary>
        /// <returns></returns>
        public IServiceProvider BuildService()
        {
            var services = new ServiceCollection();
            DiagnosticListener.AllListeners.Subscribe(new CommandListener());


            services.ServerDependencies(); //��������ע��  
//            services.AddAutoMapperSetup();
            services.AddMemoryCache();
            services.AddMvc();

            #region Autofac  ��ʱ����service��repoitory��������ʹ��

            var builder = new ContainerBuilder();
            builder.Populate(services);//��ԭ����ע������ȥ
            builder.RegisterType<BlogLogAOP>();//����ֱ���滻������������һ��Ҫ������������ע��
            builder.RegisterType<BlogCacheAOP>();
            builder.ServerDependenciesAutofac();
            var applicationContainer = builder.Build();//����������

            #endregion

            return new AutofacServiceProvider(applicationContainer);//������
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
