using D3.Blog.Domain.Core.Events;

namespace D3.Blog.Domain.Events.ArticleEvent
{
    public class ArticleAddOrEditEvent: Event
    {
        public ArticleAddOrEditEvent(int id)
        {
            Id          = id;
            AggregateId = id;//聚合根id，暂时为实体id
        }
        public int Id { get; set; }
    }
}