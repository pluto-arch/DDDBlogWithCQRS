using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using D3.Blog.Domain.Enums;
using D3.Blog.Domain.Infrastructure;

namespace D3.Blog.Application.ViewModels.Article
{


    /// <summary>
    /// 文章新增model
    /// </summary>
    public class NewArticleModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        [MaxLength(300)]
        [Required]
        public string Title { get; set; }
        /// <summary>
        /// markdown格式内容
        /// </summary>
        [Required]
        public string ContentMd { get; set; }
        /// <summary>
        /// html格式内容
        /// </summary>
        [Required]
        public string ContentHtml { get; set; }
        /// <summary>
        /// 非原创时的原作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 非原创时的外部链接
        /// </summary>
        public string ExternalUrl { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        [Required]
        public string Tags { get; set; }
        /// <summary>
        /// 博客类型：1原创。2转载。3翻译....
        /// </summary>
        [Required]
        public ArticleSource BlogType { get; set; }
        /// <summary>
        /// 文章类型
        /// </summary>
        [Required]
        public string ArticleType { get; set; }
        /// <summary>
        /// 文章的状态：0已经发布。1保存为草稿。3审核中
        /// </summary>
        [DefaultValue(ArticleStatus.Publish)]
        public ArticleStatus Status { get; set; }
        /// <summary>
        /// 创建事件
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; }
        
    }

    /// <summary>
    /// 文章编辑model
    /// </summary>
    public class EditArticleModel
    {

    }


    /// <summary>
    /// 文章查看model
    /// </summary>
    public class ArticleViewModel
    {
        /// <summary>
        /// 构造函数赋值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="contentMd"></param>
        /// <param name="contentHtml"></param>
        /// <param name="author"></param>
        /// <param name="tags"></param>
        /// <param name="blogType"></param>
        /// <param name="articleType"></param>
        /// <param name="status"></param>
        /// <param name="createTime"></param>
        /// <param name="viewCount"></param>
        /// <param name="commonCount"></param>
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
        /// <summary>
        /// 文章编号
        /// </summary>
        public int Id { get;private set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// md格式内容
        /// </summary>
        public string ContentMd { get;private set; }
        /// <summary>
        /// html格式内容
        /// </summary>
        public string ContentHtml { get;private set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get;private set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tags { get;private set; }
        /// <summary>
        /// 博客类型：1原创。2转载。3翻译....
        /// </summary>
        public ArticleSource? BlogType { get; private set; }
        /// <summary>
        /// 文章类型
        /// </summary>
        public string ArticleType { get;private set; }
        /// <summary>
        /// 文章的状态：0已经发布。1保存为草稿。3审核中
        /// </summary>
        public ArticleStatus Status { get;private set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get;private set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        public int ViewCount { get; private set; }
        /// <summary>
        /// 评论次数
        /// </summary>
        public int CommonCount { get;private set; }

    }


}