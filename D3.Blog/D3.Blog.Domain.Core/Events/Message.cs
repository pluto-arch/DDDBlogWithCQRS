using System;
using MediatR;

namespace D3.Blog.Domain.Core.Events
{
    public abstract class Message : IRequest
    {
        public string MessageType { get; protected set; }
        /// <summary>
        /// 聚合跟id
        /// </summary>
        public int   AggregateId { get; protected set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}
