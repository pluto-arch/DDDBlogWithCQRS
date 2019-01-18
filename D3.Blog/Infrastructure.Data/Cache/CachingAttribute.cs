using System;

namespace Infrastructure.Cache
{
    /// <summary>
    /// 缓存特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CachingAttribute : Attribute
    {
        /// <summary>
        /// 缓存绝对过期时间
        /// </summary>
        public int AbsoluteExpiration { get; set; } = 30;
    }
}