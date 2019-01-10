using System.Threading;
using System.Threading.Tasks;
using D3.Blog.Domain.Events.ArticleEvent;
using MediatR;

namespace D3.Blog.Domain.EventHandlers.ArticleEventHandler
{
    public class ArticleEventHandler
        : INotificationHandler<ArticleAddOrEditEvent>
    {
        public Task Handle(ArticleAddOrEditEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}