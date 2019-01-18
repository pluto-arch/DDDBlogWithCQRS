using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using D3.Blog.Application.Interface;
using D3.Blog.Application.ViewModels.Article;
using D3.Blog.Domain.Commands.Articles;
using D3.Blog.Domain.Core.BUS;
using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.Enums;
using D3.Blog.Domain.Infrastructure.IRepositorys;
using Infrastructure.Cache;
using Infrastructure.Data.Repository.EventSourcing;

namespace D3.Blog.Application.Services.Articles
{
    /// <summary>
    /// 文章Service
    /// </summary>
    public class ArticleService : IArticleService
    {
        /// <summary>
        /// automapper对象
        /// </summary>
        private readonly IMapper  _mapper;
        /// <summary>
        /// Article仓储
        /// </summary>
        private readonly IArticleRepository _articleRepository;
        ///// <summary>
        ///// 事件存储对象
        ///// </summary>
        //private readonly IEventStoreRepository _eventStoreRepository;
        /// <summary>
        /// 总线
        /// </summary>
        private readonly IMediatorHandler      Bus;
        /// <summary>
        /// 日志
        /// </summary>
        public readonly Serilog.ILogger  _logger;

        public ArticleService(
            IMapper               mapper,
            IArticleRepository   articleRepository,
            IMediatorHandler      bus,
            Serilog.ILogger  logger)
        {
            _mapper               = mapper;
            _articleRepository   = articleRepository;
            Bus                   = bus;
            _logger = logger;
        }



        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        public void Add(NewArticleModel model)
        {
            try
            {
                var registerCommand = _mapper.Map<AddNewArticleCommand>(model);
                Bus.SendCommand(registerCommand);
            }
            catch (Exception e)
            {
                //异常记录
                _logger.Error(e,$"发生错误：{e.Message}");
            }
        }

        public void Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(NewArticleModel updatemodel)
        {
            throw new System.NotImplementedException();
        }

        /********************************************************/


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
                _logger.Error(e,$"发生错误：{e.Message}");
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
                _logger.Error(e,$"发生错误：{e.Message}");
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
                _logger.Error(e,$"发生错误：{e.Message}");
                return null;
            }
        }
    }
}