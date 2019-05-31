using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using D3.Blog.Application.Interface;
using D3.Blog.Application.ViewModels.Article;
using D3.Blog.Domain.Commands.Articles;
using D3.Blog.Domain.Core.BUS;
using D3.Blog.Domain.Core.Notifications;
using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.Enums;
using D3.Blog.Domain.Infrastructure;
using D3.Blog.Domain.Infrastructure.IRepositorys;
using Infrastructure.Cache;
using Infrastructure.Data.Repository.EventSourcing;
using Infrastructure.NLoger;

namespace D3.Blog.Application.Services.Articles
{
    /// <summary>
    /// 文章Service
    /// </summary>
    public partial class ArticleService : IArticleService
    {
        /// <summary>
        /// automapper对象
        /// </summary>
        private readonly IMapper  _mapper;

        private readonly IRepository<Article> _articleRepository;

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

        public ArticleService(
            IMapper               mapper,
            IMediatorHandler      bus,
            ICustomerLogging  logger, IUnitOfWork uow)
        {
            _mapper               = mapper;
            Bus                   = bus;
            _logger = logger;
            _articleRepository = uow.GetRepository<Article>();
        }



        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        public void Add(NewArticleModel model)
        {
            try
            {
                var registerCommand = _mapper.Map<AddNewArticleCommand>(model);
                Bus.SendCommand(registerCommand);
            }
            catch (Exception e)
            {
                Bus.RaiseEvent(new DomainNotification("","出现错误，请稍后重试"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void Remove(int id)
        {
            try
            {
                Bus.SendCommand(new DeleteArticleCommand(id));
            }
            catch (Exception e)
            {
                Bus.RaiseEvent(new DomainNotification($"error", "出现错误，请稍后重试"));
            }
        }

        
        public void Update(NewArticleModel updatemodel)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 审核帖子
        /// </summary>
        /// <param name="id">帖子id</param>
        /// <param name="status">审核状态</param>
        /// <param name="errorMsg"></param>
        public void PassArticle(int id, int status,string errorMsg)
        {
            try
            {
                var model= _articleRepository.FindById(id);
                if (model!=null)
                {
                    model.Status = (ArticleStatus) status;
                    model.ErrorReason = errorMsg;
                    Bus.SendCommand(new ApprovalArticleCommand(model));
                }
                else
                {
                    Bus.RaiseEvent(new DomainNotification($"ApprovalArticleCommand", "文章不存在！"));
                }
               
            }
            catch (Exception e)
            {
                Bus.RaiseEvent(new DomainNotification($"ApprovalArticleCommand", "出现错误，请稍后重试"));
            }
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
        // ~ArticleService() {
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

        /********************************************************/



    }
}