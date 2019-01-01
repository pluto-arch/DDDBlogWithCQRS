using System;
using System.Threading;
using System.Threading.Tasks;
using D3.Blog.Domain.Commands.Customer;
using D3.Blog.Domain.Core.BUS;
using D3.Blog.Domain.Core.Notifications;
using D3.Blog.Domain.Events;
using D3.Blog.Domain.Infrastructure;
using D3.Blog.Domain.Infrastructure.IRepositorys;
using MediatR;

namespace D3.Blog.Domain.CommandHandlers.Customer
{
    public class CustomerCommandHandler
        : CommandHandler,
          IRequestHandler<RegisterNewCustomerCommand>,
          IRequestHandler<UpdateCustomerCommand>,
          IRequestHandler<RemoveCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMediatorHandler    Bus;

        public CustomerCommandHandler(ICustomerRepository  customerRepository, 
                                      IUnitOfWork     uow,
                                      IMediatorHandler  bus,
                                      INotificationHandler<DomainNotification> notifications) :base(uow, bus, notifications)
        {
            _customerRepository = customerRepository;
            Bus                 = bus;
        }
        #region 新增
        /// <summary>
        /// 新增操作
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Unit> Handle(RegisterNewCustomerCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Unit.Task;
            }

            var customer = new Entitys.Customer(message.Name, message.Email, message.BirthDate);

            if (_customerRepository.GetByEmail(customer.Email) != null)
            {
                Bus.RaiseEvent(new DomainNotification(message.MessageType, "The customer e-mail has already been taken."));
                return Unit.Task;
            }
            
            _customerRepository.Insert(customer);

            if (Commit())
            {
                //提交成功，发布领域事件
                Bus.RaiseEvent(new CustomerRegisteredEvent(customer.Id, customer.Name, customer.Email, customer.BirthDate));
            }

            return Unit.Task;
        }
        #endregion
        

        #region 更新
        /// <summary>
        /// 更新操作
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Unit> Handle(UpdateCustomerCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Unit.Task;
            }

            var customer = new Entitys.Customer(message.Id,message.Name, message.Email, message.BirthDate);
            var existingCustomer = _customerRepository.GetByEmail(customer.Email);

            ////如果按邮箱查出来的customer的id和传过来的不相等
            if (existingCustomer != null && existingCustomer.Id != customer.Id)
            {
                //和查出来的对象不相等
                if (!existingCustomer.Equals(customer))
                {
                    //发布通知，这个邮箱被占用了
                    Bus.RaiseEvent(new DomainNotification(message.MessageType,"The customer e-mail has already been taken."));
                    return Unit.Task;
                }
            }
            //否则更新
            _customerRepository.Update(customer);
            

            if (Commit())
            {
                //提交成功，发布领域事件
                Bus.RaiseEvent(new CustomerUpdatedEvent(customer.Id, customer.Name, customer.Email, customer.BirthDate));
            }

            return Unit.Task;
        }
        #endregion


        #region 删除
        public async Task<Unit> Handle(RemoveCustomerCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);//激活验证错误通知
                return Unit.Value;
            }
            try
            {
                _customerRepository.DeleteById(request.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //发布异常通知
                await Bus.RaiseEvent(new DomainNotification(e.Source, e.Message));
            }
            

            if (Commit())
            {
                //提交成功，发布领域事件
                await Bus.RaiseEvent(new CustomerDeletedEvent(request.Id));
            }
            return Unit.Value;

        }
        #endregion

       
    }
}
