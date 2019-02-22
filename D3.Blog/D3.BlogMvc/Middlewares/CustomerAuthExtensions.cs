using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;

namespace D3.BlogMvc.Middlewares
{
    /// <summary>
    /// 自定义异地登录处理中间件
    /// </summary>
    public static class CustomerAuthExtensions
    {
        public static IApplicationBuilder UseCustomerAuth(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof (app));
            return app.UseMiddleware<CustomerAuthMiddleware>(Array.Empty<object>());
        }
    }
}