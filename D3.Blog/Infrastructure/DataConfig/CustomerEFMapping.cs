using D3.Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using D3.Blog.Domain.Entitys;

namespace Infrastructure.Data.DataConfig
{
    /// <summary>
    /// Product实体映射
    /// </summary>
    public class CustomerEFMapping : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                   .HasColumnName("Name")
                   .HasMaxLength(200);

            builder.Property(e => e.BirthDate)
                   .HasColumnName("Birthday");

            builder.Property(e => e.Email)
                .IsRequired();

//            //配置要用作乐观并发令牌的属性 IsConcurrencyToken指定跟踪属性.控制某个列的数据不会出现脏操作
//            builder.Property(p => p.RowVersion).IsConcurrencyToken();
        }
    }
}
