using System.Threading.Tasks;
using D3.Blog.Domain.Core.BUS;
using D3.Blog.Domain.Core.Commands;
using D3.Blog.Domain.Core.Events;
using MediatR;

namespace Infrastructure.Data.Bus
{
    public sealed class InMemoryBus: IMediatorHandler
    {
        private readonly IMediator   _mediator;
        private readonly IEventStore _eventStore;

        public InMemoryBus(IMediator mediator, IEventStore eventStore)
        {
            _mediator   = mediator;
            _eventStore = eventStore;
        }
        /// <summary>
        /// 领域命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }
        /// <summary>
        /// 领域事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        /// <returns></returns>
        public Task RaiseEvent<T>(T @event) where T : Event
        {
            if (!@event.MessageType.Equals("DomainNotification"))
                _eventStore?.Save(@event);//将非通知事件都持久化
            return _mediator.Publish(@event);//执行对应事件
        }

    }
}
