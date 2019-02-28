using System;
using System.ComponentModel.DataAnnotations;

namespace D3.Blog.Application.ViewModels.PostGroup
{
    /// <summary>
    /// 新增viewmodel
    /// </summary>
    public class PostGroupViewModel
    {
        public PostGroupViewModel(string groupName, int owinUser)
        {
            GroupName = groupName ;
            OwinUser = owinUser ;
        }
        [MaxLength(100)]
        public string GroupName { get; set; }
        public int OwinUser { get; set; }
    }

    /// <summary>
    /// 查看编辑viewmodel
    /// </summary>
    public class ShowPostGroupViewModel
    {
        public ShowPostGroupViewModel(int id,string groupName, int owinUser)
        {
            Id=id;
            GroupName = groupName ;
            OwinUser = owinUser ;
        }

        public int Id { get; set; }
        [MaxLength(100)]
        public string GroupName { get; set; }
        public int OwinUser { get; set; }
    }

}