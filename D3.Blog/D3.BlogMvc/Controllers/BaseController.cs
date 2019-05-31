using D3.Blog.Domain.Core.Notifications;
using Infrastructure.Data.Database;
using Infrastructure.Identity.Models;
using Infrastructure.NLoger;
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
        public readonly DomainNotificationHandler _notifications;
        public readonly ICustomerLogging _logger;

        public BaseController(UserManager<AppBlogUser> userManager, RoleManager<AppBlogRole> roleManager, SignInManager<AppBlogUser> signInManager, INotificationHandler<DomainNotification> notifications,ICustomerLogging logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _notifications = (DomainNotificationHandler)notifications;
            _logger = logger;
        }
    }
}
