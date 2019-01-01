using System;
using D3.BlogMvc.Hubs;
using D3.BlogMvc.InitialSetup;
using Infrastructure.Data.Database;
using Infrastructure.Identity.Data;
using Infrastructure.Identity.Models;
using Infrastructure.Logging;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.QQ;
using Microsoft.AspNetCore.Authentication.WeChat;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace D3.BlogMvc
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 读取配置文件
        /// </summary>
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {


            services.ConfigureDependencies(); //配置依赖注入  
            services.ConfigureSeriLog(Configuration);//配置serilog
            services.AddAutoMapperSetup();
            #region IDentity
            services.AddDbContext<AppIdentityDbContext>(
                options =>options.UseSqlServer(Configuration.GetConnectionString("ID_DBCONN")));
            services.AddIdentity<AppBlogUser, AppBlogRole>()
                    .AddEntityFrameworkStores<AppIdentityDbContext>()
                    .AddDefaultTokenProviders();
            #region Password Strength Setting
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit           = false;
                options.Password.RequiredLength         = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase       = false;
                options.Password.RequireLowercase       = false;
                options.Password.RequiredUniqueChars    = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan  = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers      = true;
                 
                // User settings
                options.User.RequireUniqueEmail = true;
            }).ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath   = "/Account/AccessDenied";
                options.Cookie.Name        = "zylblog";
                options.Cookie.HttpOnly    = true;
                options.ExpireTimeSpan     = TimeSpan.FromMinutes(60);
                options.LoginPath          = "/Account/Login";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration  = true;
            });
            #endregion

            #endregion

            #region 身份验证 
            services.AddAuthentication().AddQQ(qqOptions =>
            {
                qqOptions.AppId = Configuration["Authentication:QQ:AppId"];
                qqOptions.AppKey = Configuration["Authentication:QQ:AppKey"];
            }).AddWeChat(wechatOptions =>
            {
                wechatOptions.AppId = Configuration["Authentication:WeChat:AppId"];
                wechatOptions.AppSecret = Configuration["Authentication:WeChat:AppSecret"];
            }).AddGoogle(googleOptions =>
                         {
                             googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                             googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                         });
            #endregion

            services.AddMvc();
            services.AddSignalR();
        }

        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Exception/Index");
            }
            else
            {            
                app.UseExceptionHandler("/Exception/Index");
                app.UseHsts();
            }
            

            app.UseStaticFiles();
            app.UseStatusCodePagesWithRedirects("/Exception/ErrorStatusCode/{0}");//http 错误状态码页面
            //app.UseStatusCodePages();
            app.UseHttpsRedirection();

            app.UseAuthentication();


            app.UseSignalR(routes =>
            {
                routes.MapHub<D3BlogHub>("/D3BlogHub");
            });

            app.UseMvc(router =>
            {
                router.MapRoute(
                    name: "Default",
                    template: "{controller=Home}/{action=HomePage}/{id?}");
                router.MapAreaRoute(
                       name: "areas",
                       template: "{area:exists}/{controller=Home}/{action=Index}/{id?}",
                       areaName:"Admin");
            });
        }
    }
}
