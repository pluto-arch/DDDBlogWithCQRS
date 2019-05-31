using System.ComponentModel;

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
        [Description("已发布")]
        Publish =0,
        /// <summary>
        /// 保存为草稿
        /// </summary>
        [Description("草稿")]
        Savedraft =1,
        /// <summary>
        /// 审核中
        /// </summary>
        [Description("审核中")]
        Verify =3,
        /// <summary>
        /// 已删除（回收箱）
        /// </summary>
        [Description("回收箱")]
        Deleted =4,


        /// <summary>
        /// 已删除（回收箱）
        /// </summary>
        [Description("审核失败")]
        VerifyFailed = 4,
    }
}