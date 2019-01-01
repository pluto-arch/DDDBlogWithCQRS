using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace D3.BlogMvc.Models.AccountModels
{
    /// <summary>
    /// 登录模型
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [MinLength(6)]
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// 记住我
        /// </summary>
        public bool RememberMe { get; set; }

    }
}
