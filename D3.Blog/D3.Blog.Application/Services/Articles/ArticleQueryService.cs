using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using D3.Blog.Application.ViewModels.Article;
using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.Enums;

namespace D3.Blog.Application.Services.Articles
{
    /// <summary>
    /// 文章查询部分
    /// </summary>
    public partial class ArticleService
    {
        /// <summary>
        /// 根据id查找记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ArticleViewModel> GetById(int id)
        {
            try
            {
                var a= await _articleRepository.FindByIdAsync(id);
                return _mapper.Map<ArticleViewModel>(a);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ArticleViewModel GetByFilter(Expression<Func<Article, bool>> expression)
        {
            try
            {
                var a = _articleRepository.FindByClause(expression);
                return _mapper.Map<Article,ArticleViewModel>(a);
            }
            catch (Exception e)
            {
                return null;
            }
        }

//        [Caching(AbsoluteExpiration = 10)]//增加缓存特性
        public IEnumerable<ArticleViewModel> GetList<TKey>(Expression<Func<Article, bool>> expression, Expression<Func<Article, TKey>> orderby)
        {
            try
            {
                var allArticles = _articleRepository.FindListByClause<TKey>(expression,orderby).ToList();


                var result = (from a in allArticles
                    select new ArticleViewModel(
                        a.Id,
                        a.Title,
                        a.ContentMd,
                        a.ContentHtml,
                        a.Author,
                        "",
                        a.Source,
                        "",
                        ArticleStatus.Savedraft,
                        a.AddTime,
                        a.ViewCount,
                        a.PromitCount
                        )).ToList();
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <typeparam name="TKey">排序类型</typeparam>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引（1）开始</param>
        /// <param name="expression">查询条件</param>
        /// <param name="orderby">排序条件</param>
        /// <returns></returns>
        public IEnumerable<ArticleViewModel> GetListByPage<TKey>(int pageSize, int pageIndex, Expression<Func<Article, bool>> expression, Expression<Func<Article, TKey>> orderby)
        {
            try
            {
                var allArticles = _articleRepository.FindListByPage<TKey>(pageSize,pageIndex,expression,orderby).ToList();
                var result = (from a in allArticles
                    select new ArticleViewModel(
                        a.Id,
                        a.Title,
                        a.ContentMd,
                        a.ContentHtml,
                        a.Author,
                        "",
                        a.Source,
                        "",
                        ArticleStatus.Savedraft,
                        a.AddTime,
                        a.ViewCount,
                        a.PromitCount
                    )).ToList();//此处没有用automapper
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}