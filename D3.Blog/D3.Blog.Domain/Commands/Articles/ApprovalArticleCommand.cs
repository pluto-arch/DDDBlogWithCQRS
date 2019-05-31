using D3.Blog.Domain.Entitys;

namespace D3.Blog.Domain.Commands.Articles
{
    /// <summary>
    /// 审批command
    /// </summary>
    public class ApprovalArticleCommand: ArticleCommand
    {
        public ApprovalArticleCommand(Article post)
        {
            Post = post;
        }


        public Article Post { get; set; }

        public override bool IsValid()
        {
            return true;
        }
    }
}