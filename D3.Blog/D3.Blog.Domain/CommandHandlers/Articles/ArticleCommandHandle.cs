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
        private IUser _user;
        /// <summary>
        /// 日志
        /// </summary>
        public readonly Serilog.ILogger  _logger;

        public ArticleCommandHandle(
            Serilog.ILogger  logger,
            IUser user,
            IArticleRepository articleRepository,
            IUnitOfWork uow, 
            IMediatorHandler bus, 
            INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _articleRepository = articleRepository;
            _bus = bus;
            _user = user;
            _logger = logger;
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
                article.AddUserId = _user.Id;
                article.Author = request.Author;
                if (request.CategoryId!=null)
                {
                    article.ArticleCategoryId = request.CategoryId;
                }
                article.Content = request.Content;
                article.Source = "原创";
                _articleRepository.Insert(article);
                if (Commit())
                {
                    //提交成功，发布领域事件
                    _bus.RaiseEvent(new ArticleAddOrEditEvent(article.Id));
                    var res = "";
                }
            }
            catch (Exception e)
            {
                _bus.RaiseEvent(new DomainNotification(request.MessageType,e.Message));
                _logger.Error(e,$"发生错误：{e.Message}");
            }
            return Unit.Task;
        }
    }
}