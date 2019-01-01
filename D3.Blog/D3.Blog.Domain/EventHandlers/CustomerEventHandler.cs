using System.Threading;
using System.Threading.Tasks;
using D3.Blog.Domain.Events;
using MediatR;

namespace D3.Blog.Domain.EventHandlers
{
    public class CustomerEventHandler
        :INotificationHandler<CustomerRegisteredEvent>,
        INotificationHandler<CustomerUpdatedEvent>,
         INotificationHandler<CustomerDeletedEvent>
    {
        /// <summary>
        /// 新增customer后触发的事件
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Handle(CustomerRegisteredEvent message, CancellationToken cancellationToken)
        {
            //可以刷新缓存或者其他操作
            
            return Task.CompletedTask;
        }
        /// <summary>
        /// 更新customer后激活的事件
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Handle(CustomerUpdatedEvent message, CancellationToken cancellationToken)
        {
            //可以刷新缓存或者其他操作

            return Task.CompletedTask;
        }
        /// <summary>
        /// 删除一个实体后激活此事件
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Handle(CustomerDeletedEvent notification, CancellationToken cancellationToken)
        {
            //刷新缓存或者其他操作

            return Task.CompletedTask;
        }
    }
}
