using System;
using System.Threading;
using System.Threading.Tasks;
using D3.Blog.Domain.Commands.Articles;
using D3.Blog.Domain.Core.BUS;
using D3.Blog.Domain.Core.Notifications;
using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.Enums;
using D3.Blog.Domain.Events.ArticleEvent;
using D3.Blog.Domain.Infrastructure;
using D3.Blog.Domain.Infrastructure.IRepositorys;
using MediatR;

namespace D3.Blog.Domain.CommandHandlers.Articles
{
    /// <summary>
    /// 处理程序
    /// </summary>
    public class ArticleCommandHandle
        :CommandHandler,
         IRequestHandler<AddNewArticleCommand>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMediatorHandler    _bus;
        private readonly IUser _user;
        /// <summary>
        /// 日志
        /// </summary>
//        public readonly Serilog.ILogger  _logger;

        public ArticleCommandHandle(
            IUser user,
            IArticleRepository articleRepository,
            IUnitOfWork uow, 
            IMediatorHandler bus, 
            INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _articleRepository = articleRepository;
            _bus = bus;
            _user = user;
//            _logger = logger;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Unit> Handle(AddNewArticleCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return Unit.Task;
            }

            try
            {
                var article = new Article();

                article.Title = request.Title;

                article.AddTime = request.AddTime;

                if (_user.Id != null) article.AddUserId = int.Parse(_user.Id);

                article.Author = request.Author??"";

                article.ExternalUrl = request.ExternalUrl??"";

                if (request.ArticleCategoryId!=null)
                {
                    article.ArticleCategoryId = request.ArticleCategoryId;
                }

                article.ContentMd = request.ContentMd ?? "";

                article.ContentHtml = request.ContentHtml ?? "";

                article.Source = request.Source;

                article.Status = request.Status;
                
                _articleRepository.Insert(article);
                if (Commit())
                {
                    //提交成功，发布领域事件
                    _bus.RaiseEvent(new ArticleAddOrEditEvent(article.Id));
                }
            }
            catch (Exception e)
            {
                _bus.RaiseEvent(new DomainNotification(request.MessageType,e.Message));
            }
            return Unit.Task;
        }
    }
}