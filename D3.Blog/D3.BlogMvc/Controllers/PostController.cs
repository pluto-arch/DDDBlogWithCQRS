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
using Autofac.Extensions.DependencyInjection;
using D3.Blog.Application.Interface;
using D3.Blog.Application.ViewModels;
using D3.Blog.Application.ViewModels.Article;
using D3.Blog.Application.ViewModels.PostGroup;
using D3.Blog.Domain.Core.Notifications;
using D3.Blog.Domain.Enums;
using D3.Blog.Domain.Infrastructure;
using D3.BlogMvc.Models;
using D3.BlogMvc.Models.AccountModels;
using D3.BlogMvc.Models.PostModels;
using Infrastructure.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Data.Database;
using Infrastructure.Identity.Models;
using Infrastructure.NLoger;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace D3.BlogMvc.Controllers
{
    /// <summary>
    /// 文章控制器
    /// </summary>
    [Authorize]
    public class PostController : BaseController
    {

        private readonly IArticleService _articleService;

        private readonly IPostGroupServer _postGroupServer;

        private IUser _user;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="articleService"></param>
        /// <param name="postGroupServer"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="_logger"></param>
        /// <param name="notifications"></param>
        public PostController(
            IArticleService articleService,
            IPostGroupServer postGroupServer,
            UserManager<AppBlogUser> userManager,
            RoleManager<AppBlogRole> roleManager,
            SignInManager<AppBlogUser> signInManager,
            INotificationHandler<DomainNotification> notifications,
            IUser user, ICustomerLogging _logger)
            : base(userManager, roleManager, signInManager, notifications, _logger)
        {
            _articleService = articleService;
            _postGroupServer = postGroupServer;
            _user = user;
        }

        /// <summary>
        /// 写文章页面 富文本编辑器
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult WritePost()
        {
            ViewBag.title = "YBLOG-写文章";
            ViewBag.editorType = 1;//富文本编辑器
            ViewBag.container = "container-fluid";//写文章页面和其他页面的样式控制
                                                  //读取个人系列
            var model = LoadUserGroup();
            return View(model);
        }

        /// <summary>
        /// 保存文章
        /// </summary>
        /// <param name="articleModel"></param>
        /// <param name="flag">1:发布，2:保存，其余默认保存</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult WritePost([FromForm]NewArticleModel articleModel, [FromQuery]string flag = "1")
        {
            var a = _articleService.GetByFilter(x => x.Title.Equals(articleModel.Title));
            if (a != null)
            {
                return new JsonResult("文章标题已存在，请重新输入");
            }

            NewArticleModel mo = articleModel;
            mo.CreateTime = DateTime.Now;
            switch (flag)
            {
                case "3":
                    //发布，状态变审核中
                    mo.Status = ArticleStatus.Verify;
                    break;
                case "1":
                    //只保存，不进入审核
                    mo.Status = ArticleStatus.Savedraft;
                    break;
                default:
                    mo.Status = ArticleStatus.Verify;
                    break;
            }
            if (articleModel.BlogType == ArticleSource.Original)
            {
                if (_user != null)
                {
                    mo.Author = _user.Name;
                }
            }
            _articleService.Add(mo);
            var error = _notifications.GetNotifications().Select(n => n.Value);//通知结果
            return new JsonResult("hahahah");
        }


        /// <summary>
        /// 写文章页面，markdown编辑器
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult MarkDownEditor()
        {
            ViewBag.title = "YBLOG-写文章";
            ViewBag.editorType = 2;//富文本编辑器
            ViewBag.container = "container-fluid";//写文章页面和其他页面的样式控制
            var model = LoadUserGroup();
            return View(model);
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
            ViewBag.queryValue = id;
            var model = await _articleService.GetById(int.Parse(id));
            if (model == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }
            else
            {
                Tuple<LoginModel, RegisterModel, ArticleViewModel> tmodel = new Tuple<LoginModel, RegisterModel, ArticleViewModel>(new LoginModel(), new RegisterModel(), model);
                return View(tmodel);
            }

        }

        /// <summary>
        /// 个人文章管理
        /// </summary>
        /// <param name="flag">1:全部 2:已发布 3:审核中 4:草稿箱 5:回收站</param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult ArticleManager([FromQuery]string flag = "1", [FromQuery]int pageindex = 1)
        {
            int pagesize = 10;//页大小
            ViewBag.container = "container";//写文章页面和其他页面的样式控制
            ViewBag.Title = "文章管理";
            ViewBag.flag = flag;
            ViewBag.pageindex = pageindex;
            IEnumerable<ArticleViewModel> result = new List<ArticleViewModel>();
            if (_user != null)
            {
                var uid = -1;
                if (_user.Id != null)
                {
                    uid = int.Parse(_user.Id);
                }
                switch (flag)
                {
                    case "1":
                        result = _articleService.GetList<DateTime>(x => x.AddUserId == uid, x => x.AddTime);//全部
                        break;
                    case "2":
                        result = _articleService.GetList<DateTime>(x => x.AddUserId == uid && x.IsPublish == true, x => x.AddTime);//已发布
                        break;
                    case "3":
                        result = _articleService.GetList<DateTime>(x => x.AddUserId == uid && x.Status == ArticleStatus.Verify, x => x.AddTime);//审核中
                        break;
                    case "4":
                        result = _articleService.GetList<DateTime>(x => x.AddUserId == uid && x.Status == ArticleStatus.Savedraft, x => x.AddTime);//草稿箱
                        break;
                    case "5":
                        result = _articleService.GetList<DateTime>(x => x.AddUserId == uid && x.Status == ArticleStatus.Deleted, x => x.AddTime);//回收箱
                        break;
                }
                ViewBag.totalCount = result.Count();
            }
            result = result.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();


            return View(result);
        }


        /// <summary>
        /// 个人分类管理
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult UserClassify()
        {
            ViewBag.Title = "个人分类管理";
            var postgroup = new List<ShowPostGroupViewModel>();
            var result = _postGroupServer.GetList<int>(x => x.OwinUserId == int.Parse(_user.Id));
            if (result != null)
            {
                postgroup = result.ToList();
            }
            return View(postgroup);
        }

        /// <summary>
        /// 个人分组管理
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult UserGroup()
        {
            ViewBag.Title = "个人分组管理";
            var postgroup = new List<ShowPostGroupViewModel>();
            var result = _postGroupServer.GetList<int>(x => x.OwinUserId == int.Parse(_user.Id));
            if (result != null)
            {
                postgroup = result.ToList();
            }
            return View(postgroup);
        }


        public bool IsValidOperation()
        {
            return (!_notifications.HasNotifications());
        }


        
        [HttpGet]
        public IActionResult GetData()
        {
            var postgroup = new List<ShowPostGroupViewModel>();
            var result = _postGroupServer.GetList<int>(x => x.OwinUserId == int.Parse(_user.Id));
            if (result != null)
            {
                postgroup = result.ToList();
            }
            return Json(new {total=postgroup.Count,rows =postgroup});
        }



        /// <summary>
        /// 删除个人分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult DeleteGroup(int id)
        {
             _postGroupServer.Remove(id);
             var error = _notifications.GetNotifications().Select(n => n.Value);//通知结果
             return new JsonResult(error);
        }



        /// <summary>
        /// 读取个人分组
        /// </summary>
        /// <returns></returns>
        private Tuple<D3.Blog.Application.ViewModels.Article.NewArticleModel, List<D3.Blog.Application.ViewModels.PostGroup.ShowPostGroupViewModel>> LoadUserGroup()
        {
            var postgroup = new List<ShowPostGroupViewModel>();
            var result = _postGroupServer.GetList<int>(x => x.OwinUserId == int.Parse(_user.Id));
            if (result != null)
            {
                postgroup = result.ToList();
            }
            Tuple<D3.Blog.Application.ViewModels.Article.NewArticleModel, List<D3.Blog.Application.ViewModels.PostGroup.ShowPostGroupViewModel>> model = new Tuple<D3.Blog.Application.ViewModels.Article.NewArticleModel, List<D3.Blog.Application.ViewModels.PostGroup.ShowPostGroupViewModel>>(new D3.Blog.Application.ViewModels.Article.NewArticleModel(), postgroup);
            return model;
        }
    }
}
