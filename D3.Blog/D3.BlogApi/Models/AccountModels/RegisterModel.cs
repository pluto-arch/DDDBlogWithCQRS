namespace D3.BlogApi.Models.AccountModels
{
    /// <summary>
    /// 注册模型
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name     { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email    { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
