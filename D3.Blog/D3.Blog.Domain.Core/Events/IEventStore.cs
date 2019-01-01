namespace D3.Blog.Domain.Core.Events
{
    /// <summary>
    /// 事件存储
    /// </summary>
    public interface IEventStore
    {
        void Save<T>(T theEvent) where T : Event;
    }
}
