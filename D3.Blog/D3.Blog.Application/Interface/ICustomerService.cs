using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using D3.Blog.Application.ViewModels;

namespace D3.Blog.Application.Interface
{
    /// <summary>
    /// Product Service 接口
    /// 这里使用的对象是视图对象模型
    /// </summary>
    public interface ICustomerService: IDisposable
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="model"></param>
        void Add(CustomerViewModel model);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="updatemodel"></param>
        void Update(CustomerViewModel updatemodel);
        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="id"></param>
        void Remove(int id);


        //-----------------------------------------------

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        IEnumerable<CustomerViewModel> GetAll();
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CustomerViewModel> GetById(int id);
        

    }
}
