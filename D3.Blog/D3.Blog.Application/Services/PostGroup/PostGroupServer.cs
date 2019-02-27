using System;
using AutoMapper;
using D3.Blog.Application.Interface;
using D3.Blog.Application.ViewModels.PostGroup;
using D3.Blog.Domain.Commands.PostGroup;
using D3.Blog.Domain.Core.BUS;
using D3.Blog.Domain.Core.Notifications;
using D3.Blog.Domain.Infrastructure.IRepositorys;
using Infrastructure.NLoger;

namespace D3.Blog.Application.Services.PostGroup
{
    public partial class PostGroupServer : IPostGroupServer
    {
        /// <summary>
        /// automapper对象
        /// </summary>
        private readonly IMapper  _mapper;
        /// <summary>
        /// Article仓储
        /// </summary>
        private readonly IPostGroupRepository _postGroupRepository;
        ///// <summary>
        ///// 事件存储对象
        ///// </summary>
        //private readonly IEventStoreRepository _eventStoreRepository;
        /// <summary>
        /// 总线
        /// </summary>
        private readonly IMediatorHandler      Bus;
        /// <summary>
        /// 日志
        /// </summary>
        public readonly ICustomerLogging  _logger;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="postGroupRepository"></param>
        /// <param name="bus"></param>
        /// <param name="logger"></param>
        public PostGroupServer(IMapper mapper, IPostGroupRepository postGroupRepository, IMediatorHandler bus, ICustomerLogging logger)
        {
            _mapper = mapper ;
            _postGroupRepository = postGroupRepository ;
            Bus = bus;
            _logger = logger;
        }

        public void Add(PostGroupViewModel model)
        {
            try
            {
                var registerCommand = _mapper.Map<AddPostGroupCommands>(model);
                Bus.SendCommand(registerCommand);
            }
            catch (Exception e)
            {
                Bus.RaiseEvent(new DomainNotification("","出现错误，请稍后重试"));
            }
        }

        public void Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(PostGroupViewModel updatemodel)
        {
            throw new System.NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~PostGroupServer() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}