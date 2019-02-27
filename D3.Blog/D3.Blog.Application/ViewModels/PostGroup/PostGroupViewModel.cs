using System;
using System.ComponentModel.DataAnnotations;

namespace D3.Blog.Application.ViewModels.PostGroup
{
    public class PostGroupViewModel
    {
        public PostGroupViewModel(string groupName, string owinUser)
        {
            GroupName = groupName ;
            OwinUser = owinUser ;
        }
        [MaxLength(100)]
        public string GroupName { get; set; }
        public string OwinUser { get; set; }
    }
}