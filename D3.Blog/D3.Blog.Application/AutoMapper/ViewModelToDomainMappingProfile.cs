using AutoMapper;
using D3.Blog.Application.ViewModels;
using D3.Blog.Domain.Commands.Customer;
using D3.Blog.Domain.Entitys;

namespace D3.Blog.Application.AutoMapper
{
    /// <summary>
    /// 视图模型映射成领域命令模型和领域模型
    /// </summary>
    public class ViewModelToDomainMappingProfile : Profile
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ViewModelToDomainMappingProfile ()
        {
            //视图模型 -> 添加命令模型
            //新增
            CreateMap<CustomerViewModel, RegisterNewCustomerCommand>()
               .ConstructUsing(c => new RegisterNewCustomerCommand(c.Name, c.Email,c.BirthDate));

            //更新
            CreateMap<CustomerViewModel, UpdateCustomerCommand>()
               .ConstructUsing(c => new UpdateCustomerCommand(c.Id,c.Name, c.Email,c.BirthDate));

            
        }
    }
}
