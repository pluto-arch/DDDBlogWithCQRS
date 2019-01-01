using System.IO;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Identity.Data
{
    public class AppIdentityDbContext:IdentityDbContext<AppBlogUser,AppBlogRole,int>
    {
        public AppIdentityDbContext (DbContextOptions<AppIdentityDbContext> options)
            :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}