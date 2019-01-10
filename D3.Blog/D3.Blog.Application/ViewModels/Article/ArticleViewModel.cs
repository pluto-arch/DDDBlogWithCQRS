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
        public string ContentHtml { get; set; }
        [Required]
        public string Tags { get; set; }
        [Required]
        public string BlogType { get; set; }
        [Required]
        public ArticleSource ArticleType { get; set; }
        [DefaultValue(ArticleStatus.publish)]
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

}