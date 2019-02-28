using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using D3.Blog.Application.Interface;
using D3.Blog.Application.ViewModels.PostGroup;
using D3.Blog.Domain.Core.Notifications;
using D3.Blog.Domain.Infrastructure;
using Infrastructure.Identity.Models;
using Infrastructure.NLoger;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace D3.BlogMvc.Controllers
{
    /// <summary>
    /// 用户操作控制器
    /// </summary>
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IArticleService _articleService;
        
        private readonly IPostGroupServer _postGroupServer;

        private IUser _user;


        public UserController(
            IArticleService articleService,
            IPostGroupServer postGroupServer,
            UserManager<AppBlogUser> userManager, 
            RoleManager<AppBlogRole> roleManager, 
            SignInManager<AppBlogUser> signInManager,
            INotificationHandler<DomainNotification> notifications,
            IUser user,ICustomerLogging _logger)
            : base(userManager, roleManager, signInManager, notifications,_logger)
        {
            _articleService = articleService;
            _postGroupServer = postGroupServer;
            _user = user;
        }

        /// <summary>
        /// 添加用户分组
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddUserGrout(string groupName)
        {
            PostGroupViewModel model=new PostGroupViewModel(groupName,int.Parse(_user.Id));
            _postGroupServer.Add(model);
            var error = _notifications.GetNotifications().Select(n => n.Value);//通知结果
            if (error.Any())
            {
                return RedirectToAction("UserGroup", "Post", new {isSuccess=false});
            }
            return RedirectToAction("UserGroup", "Post", new {isSuccess=true});
        }
        /// <summary>
        /// 添加用户个人分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddUserClassify(string classifyName)
        {
            var error = _notifications.GetNotifications().Select(n => n.Value);//通知结果
            if (error.Any())
            {
                return RedirectToAction("UserClassify", "Post", new {isSuccess=false});
            }
            return RedirectToAction("UserClassify", "Post", new {isSuccess=true});
        }
        
    }
}