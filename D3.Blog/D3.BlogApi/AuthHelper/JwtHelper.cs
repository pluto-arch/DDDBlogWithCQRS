using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data.Database;
using Infrastructure.Identity.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace D3.BlogApi.AuthHelper
{
   /// <summary>
    /// 生成Token，和解析Token
    /// </summary>
    public class JwtHelper
    {
        /// <summary>
        /// 配置文件
        /// </summary>
        private readonly IConfiguration _configuration;
        
        /// <summary>
        /// 构造函数，注入配置文件
        /// </summary>
        /// <param name="configuration"></param>
        public  JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// 生成jwtToken
        /// </summary>
        /// <param name="role"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        internal object GenerateJwtToken(string role, AppBlogUser user)
        {
            

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var key     = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:JWT:JwtKey"]));
            var creds   = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Authentication:JWT:JwtExpireDays"]));

            var token = new JwtSecurityToken(
                                             _configuration["Authentication:JWT:JwtIssuer"],
                                             _configuration["Authentication:JWT:JwtIssuer"],
                                             claims,
                                             expires: expires,
                                             signingCredentials: creds
                                            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    
}
