using D3.Blog.Domain;
using D3.Blog.Domain.Core.Events;
using D3.Blog.Domain.Entitys;
using Infrastructure.Data.DataConfig;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using Pomelo.EntityFrameworkCore.MySql.Storage.Internal;

namespace Infrastructure.Data.Database
{
    /// <summary>
    /// EF数据库上下文
    /// </summary>
    public class D3BlogDbContext:DbContext
    {
        /// <summary>
        /// 测试
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// 文章ef实体
        /// </summary>
        public DbSet<Article> Article { get; set; }
        /// <summary>
        /// 文章分组实体
        /// </summary>
        public DbSet<PostSeries> PostSeries { get; set; }
        /// <summary>
        /// 文章列别实体
        /// </summary>
        public DbSet<ArticleCategory> ArticleCategory { get; set; }
        /// <summary>
        /// 个人分类实体
        /// </summary>
        public DbSet<PersoalArticleCategory> PersoalArticleCategory { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CustomerEFMapping());
            builder.ApplyConfiguration(new ArticleCategoryMap());
            builder.ApplyConfiguration(new PersonalArticleCategoryMap());
            builder.ApplyConfiguration(new ArticleMap());
            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 获取链接字符串
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            
            // 使用默认的sql数据库连接
//            optionsBuilder.UseSqlServer(config.GetConnectionString("BLOG_DBCONN"));
            optionsBuilder.UseMySql(config.GetConnectionString("BLOG_MYSQL"));
        }
        
    }
}
