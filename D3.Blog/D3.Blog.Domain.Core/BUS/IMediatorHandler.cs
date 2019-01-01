using System.Threading.Tasks;
using D3.Blog.Domain.Core.Commands;
using D3.Blog.Domain.Core.Events;

namespace D3.Blog.Domain.Core.BUS
{
    public interface IMediatorHandler
    {
        Task SendCommand<T>(T command) where T : Command;
        Task RaiseEvent<T>(T  @event) where T : Event;
    }
}
