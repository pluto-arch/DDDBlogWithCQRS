using D3.Blog.Domain.Core.Notifications;
using Infrastructure.Data.Database;
using Infrastructure.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace D3.BlogMvc.Controllers
{
    public class BaseController:Controller
    {
        public readonly UserManager<AppBlogUser>   _userManager;
        public readonly RoleManager<AppBlogRole>   _roleManager;
        public readonly SignInManager<AppBlogUser> _signInManager;
        public readonly Serilog.ILogger            _logger;
        public readonly INotificationHandler<DomainNotification> _notifications;

        public BaseController(UserManager<AppBlogUser> userManager, RoleManager<AppBlogRole> roleManager, SignInManager<AppBlogUser> signInManager, ILogger logger, INotificationHandler<DomainNotification> notifications)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _notifications = notifications;
        }



    }
}
