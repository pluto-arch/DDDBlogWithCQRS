using System;
using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.Enums;

namespace D3.Blog.Domain.Commands.Articles
{
    /// <summary>
    /// 添加新文章命令
    /// </summary>
    public class AddNewArticleCommand:ArticleCommand
    {
        /// <summary>
        /// 构造函数，传入新文章数据
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="contentmd">内容Markdown格式</param>
        /// <param name="contenthtml">内容Html格式</param>
        /// <param name="addtime">添加时间</param>
        /// <param name="author">作者</param>
        /// <param name="articleCategoryId">类别</param>
        /// <param name="groupId">分组</param>
        /// <param name="source">来源：原创，转载，翻译</param>
        /// <param name="status">状态：草稿，审核中，已发布，回收箱</param>
        /// <param name="externalUrl">转载url</param>
        public AddNewArticleCommand(
            string title,
            string contentmd,
            string contenthtml,
            DateTime addtime,
            string author,
            int? articleCategoryId,
            int? groupId,
            ArticleSource source,
            ArticleStatus status,
            string externalUrl)
        {
            Title = title;
            Author = author;
            ContentMd = contentmd;
            ContentHtml = contenthtml;
            AddTime = addtime;
            Source = source;
            Status = status;
            ArticleCategoryId = articleCategoryId;
            ExternalUrl = externalUrl;
            GroupId = groupId;
        }
        /// <summary>
        /// 验证数据
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}