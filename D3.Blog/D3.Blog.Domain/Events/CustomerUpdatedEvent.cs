using System;
using D3.Blog.Domain.Core.Events;

namespace D3.Blog.Domain.Events
{
    /// <summary>
    /// customer更新激活的事件
    /// </summary>
    public class CustomerUpdatedEvent: Event
    {
        public CustomerUpdatedEvent(int id, string name, string email, DateTime birthDate)
        {
            Id          = id;
            Name        = name;
            Email       = email;
            BirthDate   = birthDate;
            AggregateId = id;
        }
        public int Id { get; set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public DateTime BirthDate { get; private set; }
    }
}
