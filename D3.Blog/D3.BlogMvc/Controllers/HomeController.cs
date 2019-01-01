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

   

    public class HomeController : BaseController
    {
        private readonly ICustomerService _customerService;
        public HomeController(ICustomerService customerService,UserManager<AppBlogUser> userManager, RoleManager<AppBlogRole> roleManager, SignInManager<AppBlogUser> signInManager, Serilog.ILogger logger, INotificationHandler<DomainNotification> notifications)
           : base(userManager, roleManager, signInManager, logger, notifications)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// 目前测试命令模式
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            ViewBag.num = "111";

            var model = await _customerService.GetById(1);

            //CustomerViewModel model = new CustomerViewModel();
            //model.Name = "张玉龙";
            //model.Email = "yulong0204@qq.com";
            //model.BirthDate = DateTime.Parse("1995-10-02");
            //_customerService.Add(model);

            //var error = _notifications.GetNotifications().Select(n => n.Value);//通知结果

            //_logger.CustomInformation(user: "小王", enviornment: "Dev", informationMessage: "添加数据成功");
            //_logger.CustomError(errorMessage: "Data Critical Added Successfully");
            //_logger.CustomFatal(fatalMessage: "Data Error Added Successfully");

            return View();
        }


        public IActionResult HomePage()
        {
            return View();
        }



        [Authorize(Roles = "eye")]
        public IActionResult Author()
        {
            return new JsonResult("这里是受限资源，需要eye角色的用户才能访问");
        }

        /// <summary>
        /// 目前测试asiox发起请求
        /// </summary>
        /// <param name="searchtext"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Search([FromForm]string searchtext)
        {
            return Json("123");
        }



        public IActionResult VueLayout()
        {
            return View();
        }

        public IActionResult Error(string id)
        {
            return Json(id);
        }


    }
}