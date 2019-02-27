using System;
using System.Threading;
using System.Threading.Tasks;
using D3.Blog.Domain.Commands.PostGroup;
using D3.Blog.Domain.Core.BUS;
using D3.Blog.Domain.Core.Notifications;
using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.Events.ArticleEvent;
using D3.Blog.Domain.Infrastructure;
using D3.Blog.Domain.Infrastructure.IRepositorys;
using MediatR;

namespace D3.Blog.Domain.CommandHandlers.PostGroup
{
    public class PostGroupCommandHandler
        : CommandHandler,
            IRequestHandler<AddPostGroupCommands>
    {
        private readonly IPostGroupRepository _groupRepository;
        private readonly IMediatorHandler    _bus;
        private readonly IUser _user;
        
        public PostGroupCommandHandler(IUnitOfWork uow, IMediatorHandler bus, INotificationHandler<DomainNotification> notifications,IPostGroupRepository groupRepository,
            IUser user) : base(uow, bus, notifications)
        {
            _groupRepository = groupRepository;
            _user = user;
            _bus = bus;
        }
        public Task<Unit> Handle(AddPostGroupCommands request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return Unit.Task;
            }
            else
            {
                try
                {
                    var postSeries = new PostSeries(request.GroupName,request.OwinUserId);
                    _groupRepository.Insert(postSeries);
                    if (Commit())
                    {
                        //提交成功，发布领域事件
                        _bus.RaiseEvent(new ArticleAddOrEditEvent(postSeries.Id));
                    }
                }
                catch (Exception e)
                {
                    _bus.RaiseEvent(new DomainNotification(request.MessageType,e.Message));
                }
            }
            return Unit.Task;
        }

    }
}