using D3.Blog.Domain.Core.Events;

namespace D3.Blog.Domain.Events.PostGroup
{
    public class PostGroupAddOrEditEvent:Event
    {
        public PostGroupAddOrEditEvent(int id)
        {
            Id          = id;
            AggregateId = id;//聚合根id，暂时为实体id
        }
        public int Id { get; set; }
    }
}