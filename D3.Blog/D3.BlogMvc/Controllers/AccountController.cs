using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using D3.BlogMvc.Hubs;
using D3.BlogMvc.Models.AccountModels;
using Infrastructure.Data.Database;
using Infrastructure.Identity.Models;
using Infrastructure.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using D3.Blog.Domain.Infrastructure;
using Infrastructure.NLoger;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;

namespace D3.BlogMvc.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppBlogUser> _userManager;
        private readonly RoleManager<AppBlogRole> _roleManager;
        private readonly SignInManager<AppBlogUser> _signInManager;
        private readonly ICustomerLogging _logger;
        private readonly IDBHelper _dbHelper;
        private readonly IUser _user;
        internal readonly IMemoryCache _memoryCache;

        public AccountController (UserManager<AppBlogUser> userManager, RoleManager<AppBlogRole> roleManager, SignInManager<AppBlogUser> signInManager,ICustomerLogging logger,IDBHelper dbHelper,IUser user,IMemoryCache memoryCache)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _dbHelper = dbHelper;
            _user = user;
            _memoryCache = memoryCache;
        }
        
        /// <summary>
        /// 新页面登录
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl="/")
        {
            ViewBag.container = "container";//写文章页面和其他页面的样式控制
            ViewBag.title = "博客-登录";
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        /// <summary>
        /// 弹窗登录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromForm]LoginModel model)
        {
            List<string> errormessage=new List<string>();
            if (ModelState.IsValid)
            {
                AppBlogUser user=await _userManager.FindByEmailAsync(model.Email);
                if (user!=null)
                {
                    var userroles = await _userManager.GetRolesAsync(user);
                    
                    await _signInManager.SignOutAsync();
                    /*await  _userManager.AddClaimAsync(user, new Claim("sbh", "12345678"));注册的时候添加，可以登陆后附加到cookie中*/
                    //第三个参数,指示在浏览器关闭后登录cookie是否应该保留的标志，true 则按照startup中配置的时间保存，否则就不保存关闭浏览器就失效。
                    var result= await _signInManager.PasswordSignInAsync(user,model.Password,model.RememberMe,false);
                    if (result.Succeeded)
                    {
                        _logger.LogLoginInfo("用户登录",nameof(LoginAsync),nameof(AccountController),null,user.UserName,user.Id.ToString());
                        _memoryCache.Set(user.UserName+"_"+user.Id, _user.ClientIP,new DateTimeOffset(DateTime.Now.AddMinutes(60)));//登陆后将客户端连接id存入缓存，缓存时间和cookie 过期时间一致
                        return new JsonResult(errormessage);
                    }
                    else if (result.IsLockedOut)
                    {
                        ModelState.AddModelError("locked", "被锁定，请联系管理员");
                    }
                    else if (result.IsNotAllowed)
                    {
                        ModelState.AddModelError("notallow", "不被允许访问");
                    }
                    else if(result.RequiresTwoFactor)
                    {
                        ModelState.AddModelError("TwoFactor", "需要双重认证");
                    }
                    else
                    {
                        ModelState.AddModelError("emailorpassworderror", "邮箱或密码错误，请重试！");
                    }
                }
                ModelState.AddModelError("nouser","用户不存在");
            }
            
            foreach (var s in ModelState.Values)
            {
                foreach (var p in s.Errors)
                {
                    errormessage.Add(p.ErrorMessage);
                }
            }
            return new JsonResult(errormessage);
        }

        /// <summary>
        /// 新页面登录 post
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm]LoginModel model, string returnUrl=null)
        {
            ViewBag.title = "博客-登录";
            ViewData["ReturnUrl"] = returnUrl;
            ViewBag.container = "container";//写文章页面和其他页面的样式控制
            if (ModelState.IsValid)
            {
                AppBlogUser user=await _userManager.FindByEmailAsync(model.Email);
                if (user!=null)
                {
                    var userroles = await _userManager.GetRolesAsync(user);
                    
                    await _signInManager.SignOutAsync();
                    /*await  _userManager.AddClaimAsync(user, new Claim("sbh", "12345678"));注册的时候添加，可以登陆后附加到cookie中*/

                    //第三个参数,指示在浏览器关闭后登录cookie是否应该保留的标志，true 则按照startup中配置的时间保存，否则就不保存关闭浏览器就失效。
                    var result= await _signInManager.PasswordSignInAsync(user,model.Password,model.RememberMe,false);
                    if (result.Succeeded)
                    {
                        _logger.LogLoginInfo($"{user.UserName} 用户登录",nameof(LoginAsync),nameof(AccountController),null,user.UserName,user.Id.ToString());
                        _memoryCache.Set(user.UserName+"_"+user.Id, _user.ClientIP,new DateTimeOffset(DateTime.Now.AddMinutes(60)));//登陆后将客户端连接id存入缓存,缓存时长和cookie 过期 时间一致
                        return LocalRedirect(returnUrl);//登录成功
                    }
                    else if (result.IsLockedOut)
                    {
                        ModelState.AddModelError("locked", "被锁定，请联系管理员");
                    }
                    else if (result.IsNotAllowed)
                    {
                        ModelState.AddModelError("notallow", "不被允许访问");
                    }
                    else if(result.RequiresTwoFactor)
                    {
                        ModelState.AddModelError("TwoFactor", "需要双重认证");
                    }
                    else
                    {
                        ModelState.AddModelError("emailorpassworderror", "邮箱或密码错误，请重试！");
                    }
                }
                ModelState.AddModelError("nouser","用户不存在");
            }            
            return View("Login",model);
            
        }



        #region 外部登录
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties  = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                // _logger.LogInformation($"Error from external provider: {remoteError}");
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                // _logger.LogInformation("用户使用 {Name} 登录了.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("lockedout","被锁定");
                // _logger.LogInformation("被锁定.");
                return RedirectToAction(nameof(Login));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"]     = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("Register", new RegisterModel{ Email = email });
            }
        }

        #endregion


        /// <summary>
        /// 页面注册
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]    
        public IActionResult Register(string returnUrl="/")
        {
            ViewBag.container = "container";//写文章页面和其他页面的样式控制
            ViewBag.title = "博客-注册";
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// 新页面注册
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]  
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm]RegisterModel model,string returnUrl="/")
        {
            if (ModelState.IsValid)
            {
                var user=await _userManager.FindByEmailAsync(model.Email);
                if (user!=null)
                {
                    ModelState.AddModelError("EmailExist", "邮箱已被使用");
                    return View(model);
                }
                var user2 = await _userManager.FindByNameAsync(model.Name);
                if (user2!=null)
                {
                    ModelState.AddModelError("NameExist","用户名已被使用");
                    return View(model);
                }
                var  newuser=new AppBlogUser
                {
                    UserName = model.Name,
                    Email = model.Email
                };
                IdentityResult issuccess= await _userManager.CreateAsync(newuser, model.Password);
                if (issuccess.Succeeded)
                {
                    await _signInManager.PasswordSignInAsync(newuser,model.Password,false,false);
                    _logger.LogLoginInfo($"{newuser.UserName} 用户注册并登录",nameof(RegisterAsync),nameof(AccountController),null,newuser.UserName,_user.Id.ToString());
                    return RedirectToLocal(returnUrl);
                }

            }

            return View(model);
        }


        /// <summary>
        /// 弹窗注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync([FromForm]RegisterModel model)
        {
            List<string> errormessage=new List<string>();
            if (!ModelState.IsValid)
            {
                foreach (var s in ModelState.Values)
                {
                    foreach (var p in s.Errors)
                    {
                        errormessage.Add(p.ErrorMessage);
                    }
                }
                return new JsonResult(errormessage);
            }
            var user=await _userManager.FindByEmailAsync(model.Email);
            if (user!=null)
            {
                errormessage.Add("邮箱已被使用");
                return new JsonResult(errormessage);
            }
            var user2 = await _userManager.FindByNameAsync(model.Name);
            if (user2!=null)
            {
                errormessage.Add("用户名已被使用");
                return new JsonResult(errormessage);
            }

            var  newuser=new AppBlogUser
            {
                UserName = model.Name,
                Email = model.Email
            };
            IdentityResult issuccess= await _userManager.CreateAsync(newuser, model.Password);
            if (issuccess.Succeeded)
            {
                await _signInManager.PasswordSignInAsync(newuser,model.Password,false,false);
                _logger.LogLoginInfo($"{newuser.UserName} 用户注册并登录",nameof(RegisterAsync),nameof(AccountController),null,newuser.UserName,_user.Id.ToString());
                return new JsonResult(errormessage);
            }
            foreach (var s in issuccess.Errors)
            {
                errormessage.Add(s.Description);
            }
            return new JsonResult(errormessage);
        }

        
        public async Task<IActionResult> Logout(string returnUrl=null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            await _signInManager.SignOutAsync();
            var endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var utime=(DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            DateTime TranslateDate = startTime.AddSeconds(utime);
            var res= _dbHelper.ExecSqlStoredProcedure("call login_time_length('"+TranslateDate.ToString("yyyy-MM-dd HH:mm:ss")+"',"+_user.Id+",@s);");
            var userOnLineTimes = "";
            if (res!=null&&res.Tables.Count>0)
            {
               userOnLineTimes = res.Tables[0].Rows[0].ItemArray[0].ToString();           
            }
            _memoryCache.Remove(_user.Name+"_"+_user.Id); //注销时移除client 连接id
            _logger.LogCustomerInfo($"{_user.Name} 注销登录,在线时长:{userOnLineTimes}",nameof(LoginAsync),nameof(AccountController),null);
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("HomePage", "Home");
            }
        }



        public IActionResult AccessDenied()
        {
            ViewBag.container = "container";//写文章页面和其他页面的样式控制
            return new JsonResult("访问受限");
        }

        private IActionResult RedirectToLocal(string returnUrl=null)
        {
            ViewBag.container = "container";//写文章页面和其他页面的样式控制
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

    }
}