using AutoMapper;
using D3.Blog.Application.ViewModels;
using D3.Blog.Application.ViewModels.Article;
using D3.Blog.Domain.Commands.Articles;
using D3.Blog.Domain.Commands.Customer;
using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.Enums;
using D3.Blog.Domain.Infrastructure;

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

            //文章新增
            CreateMap<NewArticleModel, AddNewArticleCommand>()
                .ConstructUsing(c =>
                    new AddNewArticleCommand(c.Title, c.ContentMd,c.ContentHtml,c.CreateTime,c.Author, null,c.BlogType,c.Status,c.ExternalUrl));

        }
    }
}
