using System;
using System.Collections.Generic;
using System.Linq;
using D3.Blog.Domain.Core.Events;
using Infrastructure.Data.Database;

namespace Infrastructure.Data.Repository.EventSourcing
{
    public class EventStoreSqlRepository : IEventStoreRepository
    {
        private readonly EventStoreSQLContext _context;

        public EventStoreSqlRepository(EventStoreSQLContext context)
        {
            _context = context;
        }

        public IList<StoredEvent> All(int aggregateId)
        {
            return (from e in _context.StoredEvent 
                    where e.AggregateId==aggregateId
                    select e).ToList();
        }

        public void Store(StoredEvent theEvent)
        {
            _context.StoredEvent.Add(theEvent);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
