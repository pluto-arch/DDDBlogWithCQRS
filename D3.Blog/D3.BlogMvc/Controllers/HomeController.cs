using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using D3.Blog.Application.Interface;
using D3.Blog.Application.ViewModels;
using D3.Blog.Domain.Core.Notifications;
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
    public class HomeController : BaseController
    {
        private readonly IArticleService _articleService;
        public HomeController(
            IArticleService articleService,
            UserManager<AppBlogUser> userManager, 
            RoleManager<AppBlogRole> roleManager, 
            SignInManager<AppBlogUser> signInManager, 
            INotificationHandler<DomainNotification> notifications,
            ICustomerLogging _logger)
           : base(userManager, roleManager, signInManager, notifications,_logger)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// 目前测试命令模式
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            ViewBag.num = "111";
            ViewBag.container = "container";//写文章页面和其他页面的样式控制
//            var model = await _customerService.GetById(1);

            //CustomerViewModel model = new CustomerViewModel();
            //model.Name = "张玉龙";
            //model.Email = "yulong0204@qq.com";
            //model.BirthDate = DateTime.Parse("1995-10-02");
            //_customerService.Add(model);

            //var error = _notifications.GetNotifications().Select(n => n.Value);//通知结果

            //_logger.CustomInformation(user: "小王", enviornment: "Dev", informationMessage: "添加数据成功");
            //_logger.CustomError(errorMessage: "Data Critical Added Successfully");
            //_logger.CustomFatal(fatalMessage: "Data Error Added Successfully");

            return Json("1212");
        }


        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult HomePage()
        {
            ViewBag.title = "YLBlog-首页";
            ViewBag.container = "container";//写文章页面和其他页面的样式控制
            ViewBag.NewAreaLogin = false;//异地登录标志
            var postlist= _articleService.GetListByPage(10, 1, null, a => a.AddTime).ToList();

            var postlists = (from p in postlist
                select new PostListModel(
                    p.Id,
                    p.Title,
                    SubString150Length(NoHTML(p.ContentHtml)),
                    p.Author,
                    p.CreateTime,
                    p.ViewCount,
                    p.ViewCount,
                    p.CommonCount
                    )).ToList();

            var model =
                new Tuple<D3.BlogMvc.Models.AccountModels.LoginModel, D3.BlogMvc.Models.AccountModels.RegisterModel,
                    List<D3.BlogMvc.Models.PostModels.PostListModel>>(new LoginModel(), new RegisterModel(),postlists);

            return View(model);
        }

        [HttpGet]
        public IActionResult GetPostList()
        {
            var postlist= _articleService.GetListByPage(10, 2, null, a => a.AddTime).ToList();
            var postlists = (from p in postlist
                select new PostListModel(
                    p.Id,
                    p.Title,
                    SubString150Length(NoHTML(p.ContentHtml)),
                    p.Author,
                    p.CreateTime,
                    p.ViewCount,
                    p.ViewCount,
                    p.CommonCount
                )).ToList();
            return Json(postlists);
        }

        
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="searchtext"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Search([FromForm]string searchtext)
        {
            ViewBag.title = "YLBlog-搜索结果";
            ViewBag.searchKeyWord=searchtext;    
            ViewBag.container = "container";//写文章页面和其他页面的样式控制
            return View();
        }

        /// <summary>
        /// 异地登录提示页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult NewAreaLogin()
        {
            ViewBag.NewAreaLogin = true;//异地登录标志
            var postlist= _articleService.GetListByPage(10, 1, null, a => a.AddTime).ToList();

            var postlists = (from p in postlist
                select new PostListModel(
                    p.Id,
                    p.Title,
                    SubString150Length(NoHTML(p.ContentHtml)),
                    p.Author,
                    p.CreateTime,
                    p.ViewCount,
                    p.ViewCount,
                    p.CommonCount
                )).ToList();

            var model =
                new Tuple<D3.BlogMvc.Models.AccountModels.LoginModel, D3.BlogMvc.Models.AccountModels.RegisterModel,
                    List<D3.BlogMvc.Models.PostModels.PostListModel>>(new LoginModel(), new RegisterModel(),postlists);
            return View("HomePage",model);
        }

        ///   <summary>
        ///   去除HTML标记
        ///   </summary>
        ///   <param   name=”NoHTML”>包括HTML的源码   </param>
        ///   <returns>已经去除后的文字</returns>
        public static string NoHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "",
                RegexOptions.IgnoreCase);
            //删除HTML 
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "",
                RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "",
                RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"–>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!–.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"",
                RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&",
                RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<",
                RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">",
                RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ",
                RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            return Htmlstring;
        }

        public static string SubString150Length(string Htmlstring)
        {
            if (Htmlstring.Length>150)
            {
                return Htmlstring.Substring(0, 150) + "...";
            }
            else
            {
                return Htmlstring;
            }
        }

    }
}