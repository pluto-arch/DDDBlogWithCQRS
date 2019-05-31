using System;
using AutoMapper;
using D3.Blog.Application.Interface;
using D3.Blog.Application.ViewModels;
using D3.Blog.Domain.Commands.Customer;
using D3.Blog.Domain.Core.BUS;
using D3.Blog.Domain.Infrastructure;
using D3.Blog.Domain.Infrastructure.IRepositorys;

namespace D3.Blog.Application.Services.Customer
{
    /// <summary>
    /// 分部类，Customer的写命令服务
    /// </summary>
    public partial class CustomerService: ICustomerService
    {
        /// <summary>
        /// automapper对象
        /// </summary>
        private readonly IMapper               _mapper;
        /// <summary>
        /// customer仓储
        /// </summary>
        private readonly IRepository<Domain.Entitys.Customer>   _customerRepository;
        ///// <summary>
        ///// 事件存储对象
        ///// </summary>
        // private readonly IEventStoreRepository _eventStoreRepository;
        /// <summary>
        /// 总线
        /// </summary>
        private readonly IMediatorHandler      Bus;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="customerRepository"></param>
        /// <param name="bus"></param>
        public CustomerService(
            IMapper               mapper,
            ICustomerRepository   customerRepository,
            IMediatorHandler      bus,IUnitOfWork uow)
        {
            _mapper               = mapper;
            _customerRepository   = uow.GetRepository<Domain.Entitys.Customer>();
            Bus                   = bus;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        public void Add(CustomerViewModel model)
        {
            try
            {
                var registerCommand = _mapper.Map<RegisterNewCustomerCommand>(model);
                Bus.SendCommand(registerCommand);
            }
            catch (Exception e)
            {
                //异常记录
                throw e;
            }
            
        }

        /// <summary>
        /// 按id移除
        /// </summary>
        /// <param name="id"></param>
        public void Remove(int id)
        {
            try
            {
                var removeCommand = new RemoveCustomerCommand(id);
                Bus.SendCommand(removeCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="updatemodel"></param>
        public void Update(CustomerViewModel updatemodel)
        {
            try
            {
                var updatecommand = _mapper.Map<UpdateCustomerCommand>(updatemodel);
                Bus.SendCommand(updatecommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}