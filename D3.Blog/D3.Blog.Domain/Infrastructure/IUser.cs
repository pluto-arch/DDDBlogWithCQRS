using System.Collections.Generic;
using System.Security.Claims;

namespace D3.Blog.Domain.Infrastructure
{
    public interface IUser
    {
        string Id { get; }
        string Name { get; }
        string ClientIP { get; }
        bool IsAuthenticated();
        IEnumerable<Claim> GetClaimsIdentity();
    }
}