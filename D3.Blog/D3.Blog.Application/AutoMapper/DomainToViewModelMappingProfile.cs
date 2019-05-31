using System.Collections.Generic;
using AutoMapper;
using D3.Blog.Application.ViewModels;
using D3.Blog.Application.ViewModels.Article;
using D3.Blog.Application.ViewModels.PostGroup;
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


            //article转ArticleViewModel,单个
            CreateMap<Article, ArticleViewModel>()
                .ConstructUsing(c => new ArticleViewModel(c.Id,c.Title,c.ContentMd,c.ContentHtml,c.Author,"",
                    c.Source,"",c.Status,c.AddTime,c.ViewCount,c.CollectedCount,c.PromitCount,c.ErrorReason));
            //list形式
            CreateMap<List<Article>, List<ArticleViewModel>>();

            //PostSeries转PostGroupViewModel,单个
            CreateMap<PostSeries, ShowPostGroupViewModel>()
                .ConstructUsing(c => new ShowPostGroupViewModel(c.Id,c.GroupName,c.OwinUserId));
            CreateMap<List<PostSeries>, List<ShowPostGroupViewModel>>();


        }
    }
}
