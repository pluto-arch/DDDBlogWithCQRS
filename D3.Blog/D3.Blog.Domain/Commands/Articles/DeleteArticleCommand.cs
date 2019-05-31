namespace D3.Blog.Domain.Commands.Articles
{
    public class DeleteArticleCommand : ArticleCommand
    {
        public DeleteArticleCommand(int id)
        {
            Id =id;
        }
        public override bool IsValid()
        {
            return true;
        }
    }
}