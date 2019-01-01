using System;
using System.Threading;
using System.Threading.Tasks;
using D3.Blog.Domain.Commands.Articles;
using D3.Blog.Domain.Core.BUS;
using D3.Blog.Domain.Core.Notifications;
using D3.Blog.Domain.Entitys;
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
//        private IUser _user;
        public ArticleCommandHandle(
//            IUser user,
            IArticleRepository articleRepository,
            IUnitOfWork uow, 
            IMediatorHandler bus, 
            INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _articleRepository = articleRepository;
            _bus = bus;
//            _user = user;
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

            var article = new Article
            {
                Title = request.Title,
                AddTime = DateTime.Now,
                AddUserId = 1,
                Author = request.Author,
                ArticleCategoryId = request.CategoryId,
                Content = request.Content,
                ImageUrl = request.ImageUrl,
                Source = request.Source,
                SeoTitle = request.SeoTitle,
                SeoKeyword = request.SeoKeyword,
                SeoDescription = request.SeoDescription
            };

            _articleRepository.Insert(article);
            if (Commit())
            {
                //提交成功，发布领域事件
//                _bus.RaiseEvent(new CustomerRegisteredEvent(customer.Id, customer.Name, customer.Email, customer.BirthDate));
                var res = "";
            }

            return Unit.Task;

            throw new System.NotImplementedException();
        }
    }
}