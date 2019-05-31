﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using D3.Blog.Application.ViewModels.Article;
using D3.Blog.Domain.Entitys;
using Infrastructure.AOP;
using Serilog.Core;

namespace D3.Blog.Application.Interface
{
    public interface IArticleService:IDisposable
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
        /// 更新审核状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="errorMsg"></param>
        void PassArticle(int id, int status, string errorMsg);


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
        /// <param name="orderby"></param>
        /// <returns></returns>
        IEnumerable<ArticleViewModel> GetList<TKey>(Expression<Func<Article, bool>> expression,Expression<Func<Article, TKey>> orderby);

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="expression"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        IEnumerable<ArticleViewModel> GetListByPage<TKey>(int pageSize,int pageIndex,Expression<Func<Article, bool>> expression,Expression<Func<Article, TKey>> orderby,out int count);

        /// <summary>
        /// 根据条件获取单条数据
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        ArticleViewModel GetByFilter(Expression<Func<Article,bool>> expression);

    }
}