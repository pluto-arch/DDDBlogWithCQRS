using System;
using System.Collections.Generic;
using System.Text;

namespace D3.Blog.Domain.Exceptions
{
    /// <summary>
    /// 记录不存在异常
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}
