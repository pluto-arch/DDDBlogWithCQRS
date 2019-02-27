using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace D3.BlogMvc.Models.AccountModels
{
    /// <summary>
    /// 注册模型
    /// </summary>
    public class RegisterModel
    {
        [MaxLength(100,ErrorMessage = "最大长度100")]
        [Required(ErrorMessage = "名称必须")]
        public string Name { get; set; }
        [Required(ErrorMessage = "邮箱必须")]
        [EmailAddress(ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; }
        [Required(ErrorMessage = "密码必须")]
        [MinLength(6,ErrorMessage = "密码至少6位")]
        public string Password { get; set; }
    }
}
