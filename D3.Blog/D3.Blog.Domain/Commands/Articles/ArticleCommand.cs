using System;
using D3.Blog.Domain.Core.Commands;
using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.Enums;

namespace D3.Blog.Domain.Commands.Articles
{
    public abstract class ArticleCommand:Command
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
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
        /// 来源：原创/转载/翻译
        /// </summary>
        public ArticleSource Source { get; set; }
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

        #region 导航属性
        /// <summary>
        /// 类别id
        /// </summary>
        public int? CategoryId { get; set; }
        public virtual ArticleCategory ArticleCategory { get; set; }
        #endregion
    }
}