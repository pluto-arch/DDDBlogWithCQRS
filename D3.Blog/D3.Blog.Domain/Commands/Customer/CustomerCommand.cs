using System;
using D3.Blog.Domain.Core.Commands;

namespace D3.Blog.Domain.Commands.Customer
{
    public abstract class CustomerCommand:Command
    {
        public int Id { get; protected set; }

        public string Name { get; protected set; }

        public string Email { get; protected set; }

        public DateTime BirthDate { get; protected set; }
    }
}
