using System;
using D3.Blog.Domain.Core.Events;

namespace D3.Blog.Domain.Events
{
    public class CustomerDeletedEvent:Event
    {
        public CustomerDeletedEvent(int id)
        {
            Id          = id;
           
            AggregateId = id;
        }
        public int Id { get; set; }
      
    }
}
