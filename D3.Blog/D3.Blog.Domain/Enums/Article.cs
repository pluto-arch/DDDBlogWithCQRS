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
        Transport=2,
        /// <summary>
        /// 翻译
        /// </summary>
        Translate=3,
    }
    /// <summary>
    /// 文章状态
    /// </summary>
    public enum ArticleStatus
    {
        /// <summary>
        /// 已经发布
        /// </summary>
        Publish=0, 
        /// <summary>
        /// 保存为草稿
        /// </summary>
        Savedraft=1,
        /// <summary>
        /// 审核中
        /// </summary>
        Verify=3,
    }
}