using System;
using System.Collections.Generic;
using D3.Blog.Domain.Core.Events;

namespace Infrastructure.Data.Repository.EventSourcing
{
    public interface IEventStoreRepository : IDisposable
    {
        void               Store(StoredEvent theEvent);
        IList<StoredEvent> All(int   aggregateId);
    }
}
