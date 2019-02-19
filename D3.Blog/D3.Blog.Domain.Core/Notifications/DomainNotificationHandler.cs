using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace D3.Blog.Domain.Core.Notifications
{
    /// <summary>
    /// 领域通知
    /// </summary>
    public class DomainNotificationHandler : INotificationHandler<DomainNotification>
    {
        private List<DomainNotification> _notifications;

        public DomainNotificationHandler()
        {
            _notifications = new List<DomainNotification>();
        }
        /// <summary>
        /// 处理领域通知，抛向顶层
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Handle(DomainNotification notification, CancellationToken cancellationToken)
        {
            _notifications.Add(notification);

            return Task.CompletedTask;
        }
        /// <summary>
        /// 获取通知集合
        /// </summary>
        /// <returns></returns>
        public virtual List<DomainNotification> GetNotifications()
        {
            return _notifications;
        }
        /// <summary>
        /// 是否有通知
        /// </summary>
        /// <returns></returns>
        public virtual bool HasNotifications()
        {
            return GetNotifications().Any();
        }

        public void Dispose()
        {
            _notifications = new List<DomainNotification>();
        }

    }
}
