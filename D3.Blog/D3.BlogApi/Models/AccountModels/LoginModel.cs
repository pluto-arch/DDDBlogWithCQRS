using System.ComponentModel.DataAnnotations;

namespace D3.BlogApi.Models.AccountModels
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
    }
}
