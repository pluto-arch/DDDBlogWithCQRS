using System;
using System.Collections;
using System.Collections.Generic;

namespace D3.BlogMvc.Models.PostModels
{
    /// <summary>
    /// 首页postList模型
    /// </summary>
    public class PostListModel
    {
        public PostListModel(int id, string title, string describe, string author, DateTime createtime, int supports, int views, int collections)
        {
            this.id = id;
            this.title = title ?? throw new ArgumentNullException(nameof(title));
            this.describe = describe ?? throw new ArgumentNullException(nameof(describe));
            this.author = author ?? throw new ArgumentNullException(nameof(author));
            this.createtime = createtime;
            this.supports = supports;
            this.views = views;
            this.collections = collections;
        }

        public int id { get; set; }
        public string title { get; set; }

        public string describe { get; set; }

        public string author { get; set; }

        public DateTime createtime { get; set; }

        public int supports { get; set; }
        public int views { get; set; }
        public int collections { get; set; }
        
    }
}