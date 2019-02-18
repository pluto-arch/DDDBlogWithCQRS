using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using D3.Blog.Domain.Infrastructure;

namespace Infrastructure.Identity.Models
{
    /// <summary>
    /// 访问已经登陆的用户信息
    /// </summary>
    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AspNetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }


        /// <summary>
        /// 登录用户名称
        /// </summary>
        public string Name
        {
            get
            {
                try
                {
                    return _accessor.HttpContext.User.Identity.Name;
                }
                catch (Exception e)
                {
                    return String.Empty;
                }
                
            }
        }
        /// <summary>
        /// 登录用户id
        /// </summary>
        public string Id
        {
            get
            {
                try
                {
                    return _accessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                }
                catch (Exception e)
                {
                    return String.Empty;
                }
            }
        }
        /// <summary>
        /// 是否登录
        /// </summary>
        /// <returns></returns>
        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }
        /// <summary>
        /// 用户identity声明集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext.User.Claims;
        }
        /// <summary>
        /// 客户端ip
        /// </summary>
        public string ClientIP
        {
            get
            {
                return _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
        }

    }
}