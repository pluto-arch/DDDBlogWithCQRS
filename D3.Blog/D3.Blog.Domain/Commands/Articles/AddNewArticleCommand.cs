using D3.Blog.Domain.Entitys;

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
            string title,string content,string author,int articleCategoryId,string source="",string seoTitle="",
            string seoKeyword="",string seoDes="",string imgurl="")
        {
            Title = title;
            Author = author;
            Content = content;
            Source = source;
            SeoTitle = seoTitle;
            SeoKeyword = seoKeyword;
            SeoDescription = seoDes;
            ImageUrl = imgurl;
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