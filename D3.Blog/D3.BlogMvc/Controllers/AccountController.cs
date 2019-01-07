using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
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
using Microsoft.AspNetCore.Hosting.Internal;
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
        private readonly Serilog.ILogger _logger;
        private IUser _user;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        public AccountController (UserManager<AppBlogUser> userManager, RoleManager<AppBlogRole> roleManager, SignInManager<AppBlogUser> signInManager, Serilog.ILogger logger, IUser user)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _user = user;
        }
        
        /// <summary>
        /// 非弹窗登录页面
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromForm]LoginModel model, string returnUrl = null)
        {
            List<string> errormessage=new List<string>();
            if (ModelState.IsValid)
            {
                AppBlogUser user=await _userManager.FindByEmailAsync(model.Email);
                if (user!=null)
                {
                    var userroles = await _userManager.GetRolesAsync(user);
                    
                    await _signInManager.SignOutAsync();
                    /*await  _userManager.AddClaimAsync(user, new Claim("sbh", "12345678"));注册的时候添加，然后存在于数据库，可以登陆后附加到cookie中*/


                    //第三个参数,指示在浏览器关闭后登录cookie是否应该保留的标志，true 则按照startup中配置的时间保存，否则就不保存关闭浏览器就失效。
                    var result= await _signInManager.PasswordSignInAsync(user,model.Password,model.RememberMe,false);
                    if (result.Succeeded)
                    {
                        _logger.CustomInformation(user:user.UserName,other:"登录",enviornment:"Dev",host:"Dell NoteBook",informationMessage:"用户登录");
                        
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
        /// 注册页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]    
        public IActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// 注册处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync([FromForm]RegisterModel model)
        {
            var user=await _userManager.FindByEmailAsync(model.Email);
            if (user!=null)
            {
                ModelState.AddModelError("","邮箱已被使用");
                return View("Register", model);
            }
            var user2 = await _userManager.FindByNameAsync(model.Name);
            if (user2!=null)
            {
                ModelState.AddModelError("","用户名已被使用");
                return View("Register", model);
            }

            var  newuser=new AppBlogUser
            {
                UserName = model.Name,
                Email = model.Email
            };
            IdentityResult issuccess= await _userManager.CreateAsync(newuser, model.Password);
            if (issuccess.Succeeded)
            {
                return Json("success");
            }


            return new JsonResult("");
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> Logout(string returnUrl=null)
        {
            var user = User.Identity.Name;
            await _signInManager.SignOutAsync();
            _logger.CustomInformation(user,"","Dev","localhost","用户注销");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("HomePage", "Home");
            }
        }


        /// <summary>
        /// 访问受限
        /// </summary>
        /// <returns></returns>
        public IActionResult AccessDenied()
        {
            return new JsonResult("访问受限");
        }
        /// <summary>
        /// 重定向
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [AllowAnonymous]
        public IActionResult Login2()
        {
            return View();
        }

    }
}