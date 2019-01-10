namespace D3.Blog.Domain.Enums
{
    /// <summary>
    /// 文章来源
    /// </summary>
    public enum ArticleSource
    {
        /// <summary>
        /// 原创
        /// </summary>
        Original=1,
        /// <summary>
        /// 转载
        /// </summary>
        transport=2,
        /// <summary>
        /// 翻译
        /// </summary>
        translate=3,
    }
    /// <summary>
    /// 文章状态
    /// </summary>
    public enum ArticleStatus
    {
        publish=0, //发布
        savedraft=1 //保存为草稿
    }
}