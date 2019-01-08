using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using D3.Blog.Application.Interface;
using D3.Blog.Application.ViewModels;
using D3.Blog.Domain.Core.Notifications;
using Infrastructure.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Data.Database;
using Infrastructure.Identity.Models;

namespace D3.BlogMvc.Controllers
{
    /// <summary>
    /// 文章控制器
    /// </summary>
    public class PostController : BaseController
    {
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="logger"></param>
        /// <param name="notifications"></param>
        public PostController(UserManager<AppBlogUser> userManager, RoleManager<AppBlogRole> roleManager, SignInManager<AppBlogUser> signInManager, Serilog.ILogger logger, INotificationHandler<DomainNotification> notifications)
            : base(userManager, roleManager, signInManager, logger, notifications)
        {

        }

        /// <summary>
        /// 写文章页面 富文本编辑器
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult WritePost()
        {
            return View();
        }
        /// <summary>
        /// 保存文章
        /// </summary>
        /// <param name="articleModel">文章内容</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult WritePost([FromForm]string articleModel)
        { 
            return Json("ok");
        }


        /// <summary>
        /// 写文章页面，markdown编辑器
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult MarkDownEditor()
        {
            return View();
        }



        /// <summary>
        /// 文章详细页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult PostDetails([FromQuery]string id)
        {
            ViewBag.queryValue=id;
            return View();
        }



    }
}
