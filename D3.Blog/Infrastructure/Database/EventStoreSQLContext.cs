using System.IO;
using D3.Blog.Domain.Core.Events;
using Infrastructure.Data.DataConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.Database
{
    /// <summary>
    /// 事件存储数据库上下文，继承 DbContext
    /// </summary>
    public class EventStoreSQLContext: DbContext
    {
        // 事件存储模型
        public DbSet<StoredEvent> StoredEvent { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StoredEventMap());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 获取链接字符串
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // 使用默认的sql数据库连接
            optionsBuilder.UseSqlServer(config.GetConnectionString("BLOG_DBCONN"));

//            optionsBuilder.UseMySql(config.GetConnectionString("BLOG_MYSQL"));
        }
    }
}