using System.Collections.Generic;

namespace D3.Blog.Domain.Entitys
{
    /// <summary>
    /// 文章类别
    /// </summary>
    public class ArticleCategory:BaseEntity
    {
        /// <summary>
        /// EF 使用
        /// </summary>
        public ArticleCategory()
        {}

        /// <summary>
        /// 新增使用
        /// </summary>
        /// <param name="title"></param>
        /// <param name="parentId"></param>
        /// <param name="sort"></param>
        /// <param name="icon"></param>
        /// <param name="seoTitle"></param>
        /// <param name="seoKeywords"></param>
        /// <param name="seoDes"></param>
        /// <param name="isDelete"></param>
        public ArticleCategory(string title, int? parentId, int sort, string icon, string seoTitle, string seoKeywords,string seoDes, bool isDelete)
        {
            Title = title;
            ParentId = parentId;
            Sort = sort;
            Icon = icon;
            IsDelete = isDelete;
        }

        /// <summary>
        /// 非新增可使用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="parentId"></param>
        /// <param name="sort"></param>
        /// <param name="icon"></param>
        /// <param name="seoTitle"></param>
        /// <param name="seoKeywords"></param>
        /// <param name="seoDes"></param>
        /// <param name="isDelete"></param>
        public ArticleCategory(int id,string title, int? parentId, int sort, string icon, string seoTitle, string seoKeywords,string seoDes, bool isDelete)
        {
            Id = id;
            Title = title;
            ParentId = parentId;
            Sort = sort;
            Icon = icon;
            IsDelete = isDelete;
        }


        /// <summary>
        /// 类别标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 上级标题
        /// </summary>
        public int? ParentId { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 删除标志
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 导航属性一对多关系
        /// 集合属性应该是只读的
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Article> Article { get; set; }

    }
}