using D3.Blog.Domain.Core.BUS;
using D3.Blog.Domain.Core.Commands;
using D3.Blog.Domain.Core.Notifications;
using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.Infrastructure;
using MediatR;

namespace D3.Blog.Domain.CommandHandlers
{
    public class CommandHandler
    {
        private readonly IUnitOfWork               _uow;
        private readonly IMediatorHandler          _bus;    
        private readonly DomainNotificationHandler _notifications;

        public CommandHandler(IUnitOfWork uow, IMediatorHandler bus, INotificationHandler<DomainNotification> notifications)
        {
            _uow           = uow;
            _notifications = (DomainNotificationHandler)notifications;
            _bus           = bus;
        }

        /// <summary>
        /// 验证错误通知
        /// </summary>
        /// <param name="message"></param>
        protected void NotifyValidationErrors(Command message)
        {
            //遍历验证错误集合
            foreach (var error in message.ValidationResult.Errors)
            {
                message.ValidErrorInfoList.Add(error.ErrorMessage);//添加验证错误信息
                _bus.RaiseEvent(new DomainNotification(message.MessageType, error.ErrorMessage));
            }
        }

        /// <summary>
        /// 获取对应仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity:BaseEntity
        {
            return _uow.GetRepository<TEntity>();
        }


        public bool Commit()
        {
            //没通过验证，返回false
            if (_notifications.HasNotifications()) return false;


            if (_uow.Commit()) return true;

            //提交失败，出发事件
            _bus.RaiseEvent(new DomainNotification("Commit", "We had a problem during saving your data."));
            return false;
        }

    }
}
