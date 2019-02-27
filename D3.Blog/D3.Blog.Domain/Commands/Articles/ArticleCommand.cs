using System;
using D3.Blog.Domain.Core.Commands;
using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.Enums;

namespace D3.Blog.Domain.Commands.Articles
{
    public abstract class ArticleCommand:Command
    {
        public int Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 配图连接
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 内容markdown格式 md编辑器
        /// </summary>
        public string ContentMd { get; set; }
        /// <summary>
        /// 内容html格式 富文本编辑器
        /// </summary>
        public string ContentHtml { get; set; }
        /// <summary>
        /// 浏览次数
        /// </summary>
        public int ViewCount { get; set; }
        /// <summary>
        /// 收藏次数
        /// </summary>
        public int CollectedCount { get; set; }
        /// <summary>
        /// 点赞次数
        /// </summary>
        public int PromitCount { get; set; }
        /// <summary>
        /// 作者(不一定是appbloguser)
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 非原创时的外部链接
        /// </summary>
        public string ExternalUrl { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public ArticleSource Source { get; set; }
        /// <summary>
        /// 文章状态
        /// </summary>
        public ArticleStatus Status { get; set; }


        #region SEO相关
        /// <summary>
        /// seo标题
        /// </summary>
        public string SeoTitle { get; set; }
        /// <summary>
        /// seo关键字
        /// </summary>
        public string SeoKeyword { get; set; }
        /// <summary>
        /// seo描述
        /// </summary>
        public string SeoDescription { get; set; }
        #endregion
       
        /// <summary>
        /// 添加人id，一般是当前操作用户
        /// </summary>
        public int AddUserId { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 修改人id
        /// </summary>
        public int? ModifyUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }
        /// <summary>
        /// 是否置顶，默认false
        /// </summary>
        public bool IsTop { get; set; }
        /// <summary>
        /// 是否轮播显示，默认false
        /// </summary>
        public bool IsSlide { get; set; }
        /// <summary>
        /// 是否热门，默认false
        /// </summary>
        public bool IsRed { get; set; }
        /// <summary>
        /// 是否发布(审核通过)，默认false
        /// </summary>
        public bool IsPublish { get; set; }
        /// <summary>
        /// 审核人id
        /// </summary>
        public int? VerifyUserId { get; set; }

        #region 导航属性
        /// <summary>
        /// 类别id
        /// </summary>
        public int? ArticleCategoryId { get; set; }
        public ArticleCategory ArticleCategory { get; set; }

        /// <summary>
        /// 类别id
        /// </summary>
        public int? GroupId { get; set; }
        public PostSeries PostGropu { get; set; }

        #endregion
    }
}