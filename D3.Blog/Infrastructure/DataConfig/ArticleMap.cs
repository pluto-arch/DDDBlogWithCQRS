using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.DataConfig
{
    /// <summary>
    /// 文章映射配置
    /// </summary>
    public class ArticleMap: IEntityTypeConfiguration<Article>
    {
        /// <summary>
        /// 配置映射关系
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("Article");
            builder.HasKey(x => x.Id);

            builder.Property(e => e.Title).HasMaxLength(300).IsUnicode(false).IsRequired();

            builder.Property(x => x.ContentMd).HasColumnType("text").IsUnicode().IsRequired();

            builder.Property(x => x.ContentHtml).HasColumnType("text").IsUnicode().IsRequired();

            builder.Property(x => x.Author).HasMaxLength(120).IsUnicode();

            builder.Property(x => x.Source).HasMaxLength(100).IsUnicode();

            builder.Property(x => x.SeoTitle).HasMaxLength(100).IsUnicode();

            builder.Property(x => x.SeoKeyword).HasMaxLength(100).IsUnicode();

            builder.Property(x => x.SeoDescription).HasMaxLength(250).IsUnicode();

            builder.Property(x => x.ImageUrl).HasMaxLength(300).IsUnicode();

            //-----------上：一般新增必用，下边一般修改比用---------

            builder.Property(x => x.CollectedCount).HasDefaultValue(0);
            
            builder.Property(x => x.ViewCount).HasDefaultValue(0);

            builder.Property(x => x.PromitCount).HasDefaultValue(0);
            
            builder.Property(x => x.IsTop).HasDefaultValue(false);

            builder.Property(x => x.IsSlide).HasDefaultValue(false);
            
            builder.Property(x => x.IsRed).HasDefaultValue(false);

            builder.Property(x => x.IsPublish).HasDefaultValue(false);

            builder.Property(x => x.Source).IsRequired(false);

            builder.Property(x => x.Status).HasDefaultValue(ArticleStatus.Verify);

            builder.Property(e => e.ArticleCategoryId).HasColumnName("CategoryID").IsRequired(false);

            builder.HasOne(d => d.ArticleCategory)
                .WithMany(p => p.Article)
                .HasForeignKey(d => d.ArticleCategoryId)
                .HasConstraintName("FK_Products_Categories")
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}