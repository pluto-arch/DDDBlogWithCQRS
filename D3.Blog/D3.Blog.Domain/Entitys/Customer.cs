using System;
using System.ComponentModel.DataAnnotations;

namespace D3.Blog.Domain.Entitys
{
    public class Customer:BaseEntity
    {
        public Customer(int id, string name, string email, DateTime birthDate)
        {
            Id        = id;
            Name      = name;
            Email     = email;
            BirthDate = birthDate;
        }
        /// <summary>
        /// 新增使用
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="birthDate"></param>
        public Customer(string name, string email, DateTime birthDate)
        {
            Name      = name;
            Email     = email;
            BirthDate = birthDate;
        }

        // Empty constructor for EF
        protected Customer() { }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public DateTime BirthDate { get; private set; }

        //乐观并发令牌
        [Timestamp]//一个类只能有 一个此标示,标示的必须是byte[]类型.Rowversion会对整行数据进行并发检测
        public byte[] RowVersion { get; set; }
    }
}
