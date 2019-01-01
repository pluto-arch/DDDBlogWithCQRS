using System.Threading.Tasks;
using D3.Blog.Domain.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace D3.BlogMvc.ViewComponents
{
    public class SummaryViewComponent: ViewComponent
    {
        private readonly DomainNotificationHandler _notifications;
        public SummaryViewComponent(INotificationHandler<DomainNotification> notifications)
        {
            _notifications = (DomainNotificationHandler)notifications;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notificacoes = await Task.FromResult((_notifications.GetNotifications()));//获取领域通知的结果
            notificacoes.ForEach(c => ViewData.ModelState.AddModelError(string.Empty, c.Value));//传给ModelState，一般展示操作结果信息

            return View();
        }
    }
}
