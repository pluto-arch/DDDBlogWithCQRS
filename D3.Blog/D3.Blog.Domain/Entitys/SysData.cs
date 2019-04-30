using System;

namespace D3.Blog.Domain.Entitys
{
    public class SysData : BaseEntity
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Describe { get; set; }
    }
}