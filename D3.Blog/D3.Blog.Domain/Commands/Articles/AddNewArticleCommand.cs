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
        public AddNewArticleCommand(
            string title,string content,string author,int? articleCategoryId,ArticleSource source)
        {
            Title = title;
            Author = author;
            Content = content;
            Source = source;
            CategoryId = articleCategoryId;
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