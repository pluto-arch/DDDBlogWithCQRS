using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using D3.Blog.Application.Interface;
using D3.Blog.Application.ViewModels;

namespace D3.Blog.Application.Services.Customer
{
    public partial class CustomerService
    {
        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerViewModel> GetAll()
        {
            return _customerRepository.FindAll().ProjectTo<CustomerViewModel>(_mapper.ConfigurationProvider);
        }
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CustomerViewModel> GetById(int id)
        {
            try
            {
                var customer= await _customerRepository.FindByIdAsync(id);
                return _mapper.Map<CustomerViewModel>(customer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

       
       
    }
}