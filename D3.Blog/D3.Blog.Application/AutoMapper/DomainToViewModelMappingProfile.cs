using AutoMapper;
using D3.Blog.Application.ViewModels;
using D3.Blog.Domain.Entitys;

namespace D3.Blog.Application.AutoMapper
{
    /// <summary>
    /// 领域模型映射成视图模型Dto
    /// </summary>
    public class DomainToViewModelMappingProfile: Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public DomainToViewModelMappingProfile ()
        {
            //product -----> ProductViewModel
            //如果两个模型字段一样 可以直接采用
            //CreateMap<Customer, CustomerViewModel>();
            CreateMap<Customer, CustomerViewModel>();
        }
    }
}
