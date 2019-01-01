using System.Collections.Generic;
using System.Security.Claims;

namespace D3.Blog.Domain.Infrastructure
{
    public interface IUser
    {
        int Id { get; }
        string Name { get; }
        bool IsAuthenticated();
        IEnumerable<Claim> GetClaimsIdentity();
    }
}