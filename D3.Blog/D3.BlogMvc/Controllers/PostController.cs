using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using D3.Blog.Application.Interface;
using D3.Blog.Application.ViewModels;
using D3.Blog.Application.ViewModels.Article;
using D3.Blog.Domain.Core.Notifications;
using D3.Blog.Domain.Enums;
using D3.Blog.Domain.Infrastructure;
using D3.BlogMvc.Models;
using D3.BlogMvc.Models.AccountModels;
using Infrastructure.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Data.Database;
using Infrastructure.Identity.Models;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace D3.BlogMvc.Controllers
{
    /// <summary>
    /// 文章控制器
    /// </summary>
    public class PostController : BaseController
    {

        private readonly IArticleService _articleService;
        private IUser _user;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="logger"></param>
        /// <param name="notifications"></param>
        public PostController(IArticleService articleService,UserManager<AppBlogUser> userManager, RoleManager<AppBlogRole> roleManager, SignInManager<AppBlogUser> signInManager, Serilog.ILogger logger, INotificationHandler<DomainNotification> notifications,IUser user)
            : base(userManager, roleManager, signInManager, logger, notifications)
        {
            _articleService = articleService;
            _user = user;
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
        /// <param name="articleModel"></param>
        /// <param name="flag">1:发布，2:保存，其余默认保存</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult WritePost([FromForm]Search articleModel,[FromQuery]string flag="2")
        {
           
            var a = _articleService.GetByFilter(x => x.Title.Equals(articleModel.Title));
            if (a!=null)
            {
                return new JsonResult("文章标题已存在，请重新输入"); 
            }

            NewArticleModel mo=new NewArticleModel();
            mo.BlogType = articleModel.BlogType;
            mo.ArticleType = articleModel.PostType;
            mo.ContentHtml = articleModel.Content;
            mo.ContentMd = articleModel.Contentmd;
            mo.Title = articleModel.Title;
            mo.CreateTime=DateTime.Now;
            mo.Tags = articleModel.PostTag;
            mo.ExternalUrl = articleModel.ZZUrl;
            switch (flag)
            {
                case "1":
                    //发布，状态变审核
                    mo.Status = ArticleStatus.Verify;
                    break;
                case "2":
                    //只保存
                    mo.Status = ArticleStatus.Savedraft;
                    break;
                default:
                    mo.Status = ArticleStatus.Savedraft;
                    break;
            }
            if (articleModel.BlogType==ArticleSource.Original)
            {
                if (_user!=null)
                {
                    mo.Author =_user.Name;
                }
            }
            else 
            {
                mo.Author = articleModel.ExUrl;
            }
            _articleService.Add(mo);
            var error = _notifications.GetNotifications().Select(n => n.Value);//通知结果
            return new JsonResult(error);
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


        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetData()
        {
            return Json("ok");
        }


        /// <summary>
        /// 文章详细页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> PostDetails([FromRoute]string id)
        {
            ViewBag.queryValue=id;
            var model= await _articleService.GetById(int.Parse(id));
            if (model==null)
            {
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }
            else
            {  
                Tuple<LoginModel,RegisterModel,ArticleViewModel> tmodel=new Tuple<LoginModel, RegisterModel,ArticleViewModel>(new LoginModel(),new RegisterModel(), model);
                return View(tmodel);
            }
          
        }

        /// <summary>
        /// 个人文章管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult ArticleManager([FromQuery]string flag="1",[FromQuery]int pageindex=1)
        {
            int pagesize = 2;//页大小
            ViewBag.Title = "文章管理";
            ViewBag.flag = flag;

            ViewBag.pageindex = pageindex;

            IEnumerable<ArticleViewModel> result=new List<ArticleViewModel>();
            if (_user!=null)
            {
                result = _articleService.GetList<DateTime>(x => x.AddUserId == _user.Id,x=>x.AddTime);
                
            }
            return View(result);
        }



        public bool IsValidOperation()
        {
            return (!_notifications.HasNotifications());
        }
    }
}
