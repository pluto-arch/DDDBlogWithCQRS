using D3.Blog.Domain.Core.Events;
using D3.Blog.Domain.Infrastructure;
using Infrastructure.Data.Database;
using Infrastructure.Data.Repository.EventSourcing;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Infrastructure.Data.EventSourcing
{
    /// <summary>
    /// 事件存储服务类
    /// </summary>
    public class SqlEventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IUser _user;

        public SqlEventStore(IEventStoreRepository eventStoreRepository, IUser user)
        {
            _eventStoreRepository = eventStoreRepository;
            _user                 = user;
        }
        /// <summary>
        /// 保存事件模型统一方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="theEvent"></param>
        public void Save<T>(T theEvent) where T : Event
        {
            // 对事件模型序列化
            var serializedData = JsonConvert.SerializeObject(theEvent);
            var storedEvent = new StoredEvent(
                                              theEvent,
                                              serializedData,
                                              _user.Name);

            _eventStoreRepository.Store(storedEvent);
        }
    }
}
