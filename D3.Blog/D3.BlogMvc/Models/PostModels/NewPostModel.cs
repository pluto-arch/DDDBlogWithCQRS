using D3.Blog.Domain.Enums;

namespace D3.BlogMvc.Models.PostModels
{
    /// <summary>
    /// 新发帖子模型
    /// </summary>
    public class NewPostModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容（html格式）
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 内容（markdown格式）
        /// </summary>
        public string Contentmd { get; set; }
        /// <summary>
        /// 文章来源（原创，转载，翻译...）
        /// </summary>
        public ArticleSource Source { get; set; }
        /// <summary>
        /// 文章类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 文章标签（个人分类）
        /// </summary>
        public string PostTag { get; set; }
        /// <summary>
        /// 转载链接
        /// </summary>
        public string ExUrl { get; set; }
        /// <summary>
        /// 转载原作者
        /// </summary>
        public string ExAuthor { get; set; }
    }
}