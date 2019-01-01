using System;
using System.Threading.Tasks;
using D3.Blog.Domain.Entitys;

namespace D3.Blog.Domain.Infrastructure
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public interface IUnitOfWork:IDisposable
    {
        bool Commit();
    }
}
