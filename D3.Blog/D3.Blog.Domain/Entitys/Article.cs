using System;

namespace D3.Blog.Domain.Entitys
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Article:BaseEntity
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 配图连接
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
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
        /// 排序号
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 作者(不一定是appbloguser)
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }
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
        /// <summary>
        /// 添加人id
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
        public int ArticleCategoryId { get; set; }
        public ArticleCategory ArticleCategory { get; set; }
        #endregion
        

    }
}