using D3.Blog.Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.DataConfig
{
    /// <summary>
    /// 类别映射
    /// </summary>
    public class ArticleCategoryMap: IEntityTypeConfiguration<ArticleCategory>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<ArticleCategory> builder)
        {
            builder.ToTable("ArticleCategory");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Icon)
                .HasMaxLength(50)
                .IsUnicode();
            builder.Property(x => x.Title)
                .HasMaxLength(120)
                .IsRequired()
                .IsUnicode();
        }
    }

    /// <summary>
    /// 个人分类EF映射
    /// </summary>
    public class PersonalArticleCategoryMap : IEntityTypeConfiguration<PersoalArticleCategory>
    {
        public void Configure(EntityTypeBuilder<PersoalArticleCategory> builder)
        {
            builder.ToTable("PersonalCategory");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CategoryName)
                .HasMaxLength(100)
                .IsUnicode();
        }
    }
}