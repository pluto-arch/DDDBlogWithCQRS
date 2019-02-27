using System;
using D3.Blog.Application.ViewModels.PostGroup;

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
        
    }
}