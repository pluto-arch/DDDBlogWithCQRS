using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using D3.Blog.Domain.Enums;
using D3.Blog.Domain.Infrastructure;

namespace D3.Blog.Application.ViewModels.Article
{


    /// <summary>
    /// 文章新增
    /// </summary>
    public class NewArticleModel
    {
        [MaxLength(300)]
        [Required]
        public string Title { get; set; }
        [Required]
        public string ContentMd { get; set; }
        [Required]
        public string ContentHtml { get; set; }

        public string Author { get; set; }
        [Required]
        public string Tags { get; set; }
        [Required]
        public ArticleSource BlogType { get; set; }
        [Required]
        public string ArticleType { get; set; }
        [DefaultValue(ArticleStatus.Publish)]
        public ArticleStatus Status { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
        
    }
    /// <summary>
    /// 编辑
    /// </summary>
    public class EditArticleModel
    {

    }


    /// <summary>
    /// 查看model
    /// </summary>
    public class ArticleViewModel
    {
        public ArticleViewModel(int id, string title, string contentMd, string contentHtml, string author, string tags, ArticleSource? blogType, string articleType, ArticleStatus status, DateTime createTime, int viewCount, int commonCount)
        {
            Id = id;
            Title = title;
            ContentMd = contentMd;
            ContentHtml = contentHtml;
            Author = author;
            Tags = tags;
            BlogType = blogType;
            ArticleType = articleType;
            Status = status;
            CreateTime = createTime;
            ViewCount = viewCount;
            CommonCount = commonCount;
        }

        public int Id { get;private set; }
        public string Title { get; private set; }
        
        public string ContentMd { get;private set; }
        
        public string ContentHtml { get;private set; }

        public string Author { get;private set; }
        
        public string Tags { get;private set; }
        
        public ArticleSource? BlogType { get; private set; }
        
        public string ArticleType { get;private set; }
        
        public ArticleStatus Status { get;private set; }
        
        public DateTime CreateTime { get;private set; }


        public int ViewCount { get; private set; }
        public int CommonCount { get;private set; }

    }


}