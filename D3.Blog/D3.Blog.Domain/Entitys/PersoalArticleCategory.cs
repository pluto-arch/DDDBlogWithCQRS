namespace D3.Blog.Domain.Entitys
{
    /// <summary>
    /// 个人分类
    /// </summary>
    public class PersoalArticleCategory:BaseEntity
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int Uid { get; set; }
        /// <summary>
        /// 个人分类名称
        /// </summary>
        public string CategoryName  { get; set; }
    }
}