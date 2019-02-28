using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using D3.Blog.Application.ViewModels.Article;
using D3.Blog.Application.ViewModels.PostGroup;
using D3.Blog.Domain.Entitys;

namespace D3.Blog.Application.Interface
{
    public interface IPostGroupServer:IDisposable
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="model"></param>
        void Add(PostGroupViewModel model);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="updatemodel"></param>
        void Update(PostGroupViewModel updatemodel);
        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="id"></param>
        void Remove(int id);


        /*-------查询---------*/

        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ShowPostGroupViewModel> GetById(int id);
        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IEnumerable<ShowPostGroupViewModel> GetList<TKey>(Expression<Func<PostSeries, bool>> expression);

        
    }
}