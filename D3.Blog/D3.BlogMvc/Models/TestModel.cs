using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using D3.Blog.Domain.Enums;

namespace D3.BlogMvc.Models
{
    /// <summary>
    /// 测试
    /// </summary>
    public class Search
    {
        /// <summary>
        /// 搜索文本
        /// </summary>
        public string Title { get; set; }

        public string Content { get; set; }
        public ArticleSource BlogType { get; set; }
        public string PostType { get; set; }
        public string PostTag { get; set; }
        public string Contentmd { get; set; }
    }
}
