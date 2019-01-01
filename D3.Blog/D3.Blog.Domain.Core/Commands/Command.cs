using System;
using System.Collections.Generic;
using D3.Blog.Domain.Core.Events;
using FluentValidation.Results;
using MediatR;

namespace D3.Blog.Domain.Core.Commands
{
    /// <summary>
    /// 抽象命令基类
    /// </summary>
    public abstract class Command: Message
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; private set; }
        /// <summary>
        /// 验证结果，需要引用FluentValidation
        /// </summary>
        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// 定义抽象方法，是否有效
        /// </summary>
        /// <returns></returns>
        public abstract bool IsValid();

        /// <summary>
        /// 验证错误信息集合
        /// </summary>
        public List<string> ValidErrorInfoList { get; set; }
    }
}
