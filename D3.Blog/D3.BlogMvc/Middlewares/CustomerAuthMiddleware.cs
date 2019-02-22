using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;

namespace D3.BlogMvc.Middlewares
{
    /// <summary>
    /// 自定义异地登录处理中间件
    /// </summary>
    public class CustomerAuthMiddleware
    {
        private readonly RequestDelegate next;

        private IMemoryCache _cache;
        public IAuthenticationSchemeProvider Schemes { get; set; }

        public CustomerAuthMiddleware(RequestDelegate next, IMemoryCache cache, IAuthenticationSchemeProvider schemes)
        {
            this.next = next;
            _cache = cache;
            Schemes = schemes;
        }

        public async Task Invoke(HttpContext context)
        {
            var connectionId = context.Connection.Id;
            try
            {
                var uid = context.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var uname = context.User.Identity.Name;
                var cachekey = uname+"_"+uid;
                var cacheConnId = _cache.Get(cachekey);
                if (cacheConnId != null)
                {
                    if (connectionId != cacheConnId.ToString())
                    {
                        context.Response.Cookies.Delete("zylblog"); //删除客户端cookie

;                       string js = @"<script language='JavaScript'>
                                    alert('您的账户在其他地方登录，建议重新登录并修改密码');</script>";
                        context.Response.Redirect("https://localhost:5001/Home/NewAreaLogin");
                    }
                    else
                    {
                        await this.next(context);
                    }
                }
                else
                {
                    //用户注册后其实不通过查数据库无法得到uid，故无法得到缓存键，所以从cookie中获取，存到缓存中
                    AuthenticationScheme authenticateSchemeAsync = await this.Schemes.GetDefaultAuthenticateSchemeAsync();
                    if (authenticateSchemeAsync != null)
                    {
                        AuthenticateResult authenticateResult = await context.AuthenticateAsync(authenticateSchemeAsync.Name);
                        if (authenticateResult?.Principal != null)
                        {
                            uname = authenticateResult?.Principal.Identity.Name;
                            uid=authenticateResult?.Principal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                            _cache.Set(uname + "_" + uid,context.Connection.Id,new DateTimeOffset(DateTime.Now.AddMinutes(60)));
                        }
                    }
                    await this.next(context);
                }


            }
            catch (Exception e)
            {
                await this.next(context);
            }
        }

    }
}
