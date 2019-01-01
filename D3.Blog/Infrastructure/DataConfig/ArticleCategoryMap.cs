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
            builder.Property(x => x.SeoTitle)
                .HasMaxLength(120)
                .IsUnicode();
            builder.Property(x => x.SeoKeywords)
                .HasMaxLength(120)
                .IsUnicode();
            builder.Property(x => x.SeoDes)
                .HasMaxLength(200)
                .IsUnicode();
        }
    }
}