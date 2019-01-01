using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Identity.Data
{
   /// <summary>
    /// Identity 种子数据
    /// </summary>
    public class AppIdentitySeedData
    {
        public static async Task InitialData(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<AppIdentityDbContext>();

                if (!dbContext.Database.GetPendingMigrations().Any())//迁移有关，手动迁移 所以可以不用这段代码
                {
                    await dbContext.Database.MigrateAsync();

                    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<AppBlogRole>>();
                    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppBlogUser>>();

                    if (!userManager.Users.Any())
                    {
                        var user1 = new AppBlogUser
                        {
                            UserName = "jack",
                            Age = 12,
                            Birthday = DateTime.Parse("1997-03-07"),
                            Email = "lbx@qq.com",
                            Gender = "male",
                            CreateTime = DateTime.Now
                        };
                        var user2 = new AppBlogUser
                        {
                            UserName = "rose",
                            Age = 12,
                            Birthday = DateTime.Parse("1997-03-07"),
                            Email = "zyl@qq.com",
                            Gender = "female",
                            CreateTime = DateTime.Now
                        };
                        var result1 = await userManager.CreateAsync(user1, "520lbx1314");
                        var result2 = await userManager.CreateAsync(user2, "520lbx1314");

                        if (result1.Succeeded && result2.Succeeded)
                        {
                            var role1 = new AppBlogRole
                            {
                                Name = "admin",
                                CreateTime = DateTime.Now
                            };
                            var role2 = new AppBlogRole
                            {
                                Name = "user",
                                CreateTime = DateTime.Now
                            };
                            var result3 = await roleManager.CreateAsync(role1);
                            var result4 = await roleManager.CreateAsync(role2);
                            if (result3.Succeeded && result4.Succeeded)
                            {
                                var iduser1 = await userManager.FindByEmailAsync("zyl@qq.com");
                                var iduser2 = await userManager.FindByEmailAsync("lbx@qq.com");
                                await userManager.AddToRoleAsync(iduser1, "admin");
                                await userManager.AddToRoleAsync(iduser2, "user");
                            }
                        }
                    }

                   
                }
            }
        }
    }
}