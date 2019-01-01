using System;
using D3.Blog.Domain.Core.Events;

namespace D3.Blog.Domain.Events
{
    public class CustomerRegisteredEvent: Event
    {
        public CustomerRegisteredEvent(int id, string name, string email, DateTime birthDate)
        {
            Id          = id;
            Name        = name;
            Email       = email;
            BirthDate   = birthDate;
            AggregateId = id;//聚合根id，暂时为实体id
        }
        public int Id { get; set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public DateTime BirthDate { get; private set; }
    }
}
