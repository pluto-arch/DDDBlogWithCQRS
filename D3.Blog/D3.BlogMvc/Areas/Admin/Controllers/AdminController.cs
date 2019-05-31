using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using D3.Blog.Application.Interface;
using D3.Blog.Domain.Core.Notifications;
using D3.BlogMvc.Controllers;
using Infrastructure.Identity.Models;
using Infrastructure.NLoger;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace D3.BlogMvc.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,Admin")]
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppBlogUser> _userManager;
        private readonly RoleManager<AppBlogRole> _roleManager;
        private readonly SignInManager<AppBlogUser> _signInManager;
        private readonly DomainNotificationHandler _notifications;
        private readonly IArticleService _articleService;
        private readonly ICustomerLogging _logger;

        public AdminController(UserManager<AppBlogUser> userManager,
            RoleManager<AppBlogRole> roleManager,
            SignInManager<AppBlogUser> signInManager,
            INotificationHandler<DomainNotification> notifications,
            IArticleService articleService,
            ICustomerLogging logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _notifications = (DomainNotificationHandler)notifications;
            _logger = logger;
            _articleService = articleService;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            ViewBag.usercount = _userManager.Users.Count();
            ViewBag.totalCount = _articleService.GetList<DateTime>(x=>x.Id!=-1,x=>x.AddTime).Count();
            return View();
        }

        /// <summary>
        /// 文章列表
        /// </summary>
        /// <param name="index"></param>
        /// <param name="isfromDelete"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IActionResult Articles()
        {
//            ViewBag.pageIndex = index; 
//            int count = 0;
//            var articleList = _articleService.GetListByPage<DateTime>(20,index,x=>x.Id!=-1,x=>x.AddTime,out count);
//            ViewBag.totalCount = count;
            return View();
        }


        /// <summary>
        /// 文章列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IActionResult GetArticles(int pageIndex=1,int pageSize=20)
        {
            int count = 0;
            var articleList = _articleService.GetListByPage<DateTime>(20,1,x=>x.Id!=-1,x=>x.AddTime,out count);
            ViewBag.totalCount = count;
            return Json(new
            {
                code=0,
                msg="",
                count= count,
                data= articleList
            });
        }



        /// <summary>
        /// 查看文章详情
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ViewDetails(int id)
        {
            var article = await _articleService.GetById(id);
            return View(article);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteArticle(int id)
        {
            _articleService.Remove(id);
            var notifys = "";
            if (_notifications.HasNotifications())
            {
                notifys = _notifications.GetNotifications().FirstOrDefault(x => x.Key == "DeleteArticleCommand")?.Value;
                return Json(new { errorcode = 0, msg = notifys });
            }
            else
            {
                return Json(new { errorcode = -1, msg = "出现异常，请稍后再试" });
            }
        }

        /// <summary>
        /// 审核post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult AuditingPost(int id, string errorMsg, int status=3)
        {
            _articleService.PassArticle(id, status, errorMsg);
            var notifys = "";
            if (_notifications.HasNotifications())
            {
                notifys = _notifications.GetNotifications().FirstOrDefault(x => x.Key == "ApprovalArticleCommand")?.Value;
                return Json(new { errorcode = 0, msg = notifys });
            }
            else
            {
                return Json(new { errorcode = -1, msg = "出现异常，请稍后再试" });
            }

        }

    }
}