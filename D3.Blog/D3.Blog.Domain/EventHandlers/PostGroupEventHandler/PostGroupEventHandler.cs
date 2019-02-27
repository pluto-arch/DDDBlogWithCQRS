using System.Threading;
using System.Threading.Tasks;
using D3.Blog.Domain.Events.ArticleEvent;
using D3.Blog.Domain.Events.PostGroup;
using MediatR;

namespace D3.Blog.Domain.EventHandlers.PostGroupEventHandler
{
    public class PostGroupEventHandler
        : INotificationHandler<PostGroupAddOrEditEvent>
    {

        public Task Handle(PostGroupAddOrEditEvent notification, CancellationToken cancellationToken)
        {
            //可以刷新缓存或者其他操作


            return Task.CompletedTask;
        }
    }
}