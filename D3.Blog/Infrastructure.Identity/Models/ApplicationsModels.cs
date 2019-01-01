using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Models
{
    /// <summary>
    /// identity 角色
    /// </summary>
    public class AppBlogRole:IdentityRole<int>
    {
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// identity 用户
    /// </summary>
    public class AppBlogUser:IdentityUser<int>
    {
        public int       Age            { get; set; }
        public string    Gender         { get; set; }
        public string    HypeLink       { get; set; }
        public DateTime? Birthday       { get; set; }
        public string    Image          { get; set; }
        public DateTime  CreateTime     { get; set; }
        public DateTime? LastChangeTime { get; set; }
    }
}