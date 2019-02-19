using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using D3.Blog.Domain.Core.Notifications;
using Infrastructure.Data.Database;
using Infrastructure.Identity.Models;
using Infrastructure.Logging;
using Infrastructure.NLoger;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Serilog;

namespace D3.BlogMvc.Controllers
{
    [AllowAnonymous]
    public class ExceptionController : BaseController
    {
        public ExceptionController(
            UserManager<AppBlogUser> userManager, 
            RoleManager<AppBlogRole> roleManager, 
            SignInManager<AppBlogUser> signInManager, 
            INotificationHandler<DomainNotification> notifications,
            ICustomerLogging _logger)
            :base(userManager,roleManager,signInManager, notifications,_logger)
        {
        }
        /// <summary>
        /// 异常处理页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {

            ViewBag.Title = "不好意思，发生错误了！";
            ViewBag.container = "container";//写文章页面和其他页面的样式控制
            //记录异常信息
            //var statusCodePagesFeature = HttpContext.Features.Get<IStatusCodePagesFeature>();//获取状态吗
            var statusCodePagesFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();//获取异常
            _logger.LogCustomerError("出现异常",statusCodePagesFeature.Error.Source,nameof(ExceptionController),statusCodePagesFeature.Error);
//            var currentuser= User.Identity.Name;
            ViewBag.ErrorMessage = statusCodePagesFeature.Error.Message;
            return View("Error",statusCodePagesFeature);//暂时返回json  应该返回error友好界面
        }

        /// <summary>
        /// 404等状态码处理页面
        /// </summary>
        /// <returns></returns>
        public IActionResult ErrorStatusCode(int id)
        {
            ViewBag.Title = "不好意思，发生错误了！";
            ViewBag.container = "container";//写文章页面和其他页面的样式控制
            var file = Path.Combine(Directory.GetCurrentDirectory(), 
                                    "wwwroot", "statuscodepage", "welcome.html");
            if (404==id)
            {
                file = Path.Combine(Directory.GetCurrentDirectory(), 
                                    "wwwroot", "statuscodepage", "404NotFound.html");
            }
            return new PhysicalFileResult(file,new MediaTypeHeaderValue("text/html"));
            
        }
        

    }
}