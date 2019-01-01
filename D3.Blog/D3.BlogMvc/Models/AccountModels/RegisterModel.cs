using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D3.BlogMvc.Models.AccountModels
{
    /// <summary>
    /// 注册模型
    /// </summary>
    public class RegisterModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
