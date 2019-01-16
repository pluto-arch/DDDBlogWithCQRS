using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using D3.Blog.Application.ViewModels.Article;
using D3.Blog.Domain.Entitys;

namespace D3.Blog.Application.Interface
{
    public interface IArticleService
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="model"></param>
        void Add(NewArticleModel model);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="updatemodel"></param>
        void Update(NewArticleModel updatemodel);
        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="id"></param>
        void Remove(int id);


        //-----------------------------------------------

        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ArticleViewModel> GetById(int id);
        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IEnumerable<ArticleViewModel> GetList<TKey>(Expression<Func<Article, bool>> expression,Expression<Func<Article, TKey>> orderby);
        /// <summary>
        /// 根据条件获取单条数据
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        ArticleViewModel GetByFilter(Expression<Func<Article,bool>> expression);

    }
}