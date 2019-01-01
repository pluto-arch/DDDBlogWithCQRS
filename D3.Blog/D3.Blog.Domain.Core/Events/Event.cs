using System;
using MediatR;

namespace D3.Blog.Domain.Core.Events
{
    public class Event: Message, INotification
    {
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
