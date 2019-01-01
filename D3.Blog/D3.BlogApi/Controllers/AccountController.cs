using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using D3.BlogApi.AuthHelper;
using D3.BlogApi.Models.AccountModels;
using Infrastructure.Data.Database;
using Infrastructure.Identity.Models;
using Infrastructure.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace D3.BlogApi.Controllers
{
    /// <summary>
    /// 用户账户控制器
    /// </summary>
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppBlogUser>   _userManager;
        private readonly RoleManager<AppBlogRole>   _roleManager;
        private readonly SignInManager<AppBlogUser> _signInManager;
        private readonly Serilog.ILogger            _logger;
        private readonly JwtHelper _JwtHelper;

        public AccountController(UserManager<AppBlogUser> userManager, RoleManager<AppBlogRole> roleManager, SignInManager<AppBlogUser> signInManager, ILogger logger, JwtHelper JwtHelper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _JwtHelper = JwtHelper;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<object> Login([FromBody] LoginModel model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user!=null)
            {
                var result = await _signInManager.PasswordSignInAsync(user,model.Password,true,false);
            
                if (result.Succeeded)
                {
                    _logger.CustomInformation(user:user.UserName,other:"登录",enviornment:"Dev",host:"Dell NoteBook",informationMessage:"用户登录");
                    var userrole = await _userManager.GetRolesAsync(user);
                    string roles = "";
                    foreach (var r in userrole)
                    {
                        roles += r+",";
                    }
                    return  _JwtHelper.GenerateJwtToken(roles.TrimEnd(','), user);
                }
            }
            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<object> Register([FromBody] RegisterModel model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user!=null)
            {
                var usernew = new AppBlogUser
                {
                    UserName = model.Email, 
                    Email    = model.Email
                };
                var result = await _userManager.CreateAsync(usernew, model.Password);
                if (result.Succeeded)
                {
                    // var role = _userManager.AddToRoleAsync(usernew,"");//注册完成后赋予初始角色
                    await _signInManager.SignInAsync(user, false);
                    return  _JwtHelper.GenerateJwtToken(model.Email, user);
                }
            }
            throw new ApplicationException("UNKNOWN_ERROR");
        }

        
    }
}