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


    
        public string Name
        {
            get
            {
            #if DEBUG
                return "ZYL";               
            #endif
                return _accessor.HttpContext.User.Identity.Name;
            }
        }

        public int Id
        {
            get
            {
            #if DEBUG
                return 1;               
            #endif
                return int.Parse(
                    _accessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            }
        }

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return  _accessor.HttpContext.User.Claims;
        }
        
    }
}