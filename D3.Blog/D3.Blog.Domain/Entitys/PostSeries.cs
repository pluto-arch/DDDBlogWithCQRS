using System;
using System.Collections.Generic;

namespace D3.Blog.Domain.Entitys
{
    /// <summary>
    /// 文章系列，文章组
    /// </summary>
    public class PostSeries:BaseEntity
    {
        /// <summary>
        /// ef 使用
        /// </summary>
        protected PostSeries()
        {}
        /// <summary>
        /// new 使用
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="owinUserId"></param>
        public PostSeries(string groupName, int owinUserId)
        {
            GroupName = groupName;
            OwinUserId = owinUserId;
        }
        /// <summary>
        /// update 使用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupName"></param>
        /// <param name="owinUserId"></param>
        public PostSeries(int id,string groupName, int owinUserId)
        {
            Id = id;
            GroupName = groupName;
            OwinUserId = owinUserId;
        }


        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// group 所属人
        /// </summary>
        public int OwinUserId { get; set; }

        /// <summary>
        /// 导航属性一对多关系
        /// 集合属性应该是只读的
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Article> Article { get; set; }

    }
}