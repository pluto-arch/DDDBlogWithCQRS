using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using D3.BlogApi.AuthHelper;
using D3.BlogApi.InitialSetup;
using Infrastructure.AOP;
using Infrastructure.Data.Database;
using Infrastructure.Identity.Data;
using Infrastructure.Identity.Models;
using Infrastructure.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using NLog.Web;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

namespace D3.BlogApi
{
    /// <summary>
    /// api启动配置
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 构造函数注入配置文件
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 配置文件
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.ConfigureDependencies(); //配置依赖注入   
                                              //            services.ConfigureSeriLog(Configuration); //配置serilog
            services.AddAutoMapperSetup();

            #region 支持跨域


            services.AddCors();
            // services.AddCors(options =>
            //  {
            //      options.AddPolicy("allowAll", policy =>
            //      {
            //            policy.AllowAnyOrigin()    //允许任何源
            //                  .AllowAnyMethod()    //允许任何方式
            //                  .AllowAnyHeader()    //允许任何头
            //                  .AllowCredentials(); //允许cookie
            //      });
            //      //一般采用这种方法
            //      //options.AddPolicy("allowAll", policy =>
            //      //{
            //      //    policy
            //      //        .WithOrigins("http://localhost", "http://localhost:44388", "https://localhost", "https://localhost:44388")//支持多个域名端口
            //      //        .AllowAnyMethod()//请求方法添加到策略
            //      //        .WithHeaders("authorization");//标头添加到策略
            //      //});
            // });

            #endregion



            #region  swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v0.1.0",
                    Title = "D3Blog.Core API",
                    Description = "框架说明文档",
                    TermsOfService = "None",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact { Name = "D3Blog.Core", Email = "yulong0204@qq.com", Url = "" }
                });
                //F:\博客项目\D3.Blog\D3.BlogApi\D3.BlogApi.xml

                #region 读取xml信息
                var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "D3.BlogApi.xml");  //这个就是刚刚配置的xml文件名
                var xmlModelPath = Path.Combine(basePath, "D3.Blog.Application.xml"); //这个就是Model层的xml文件名
                c.IncludeXmlComments(xmlPath, true);                            //默认的第二个参数是false，这个是controller的注释，记得修改
                c.IncludeXmlComments(xmlModelPath);
                #endregion


                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { "BlogDemo", new string[] { } }, };
                c.AddSecurityRequirement(security);
                //方案名称“Blog.Core”可自定义，上下一致即可
                c.AddSecurityDefinition("BlogDemo", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入{token}\"",
                    Name = "Authorization", //jwt默认的参数名称
                    In = "header",        //jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
            });
            #endregion



            #region IDentity
            services.AddDbContext<AppIdentityDbContext>(
                        options => options.UseSqlServer(Configuration.GetConnectionString("ID_DBCONN")));
            services.AddIdentity<AppBlogUser, AppBlogRole>()
                    .AddEntityFrameworkStores<AppIdentityDbContext>()
                    .AddDefaultTokenProviders();
            #region Password Strength Setting
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;
                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;
                // User settings
                options.User.RequireUniqueEmail = true;
            })
                .ConfigureApplicationCookie(options =>
                {
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.Cookie.Name = "zylblog";
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.LoginPath = "/Account/Login";
                    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                    options.SlidingExpiration = true;
                });
            #endregion

            #endregion



            #region 认证
            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["Authentication:JWT:JwtIssuer"],
                        ValidAudience = Configuration["Authentication:JWT:JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:JWT:JwtKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });


            #endregion

            

            services.AddMemoryCache();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #region Autofac  暂时当作service和repoitory的拦截器使用
            var builder = new ContainerBuilder();
            builder.Populate(services);//将原生的注入填充进去
            builder.RegisterType<BlogLogAOP>();//可以直接替换其他拦截器！一定要把拦截器进行注册
            builder.RegisterType<BlogCacheAOP>();
            builder.ConfigureDependenciesAutofac();
            var applicationContainer = builder.Build();//构建新容器
            #endregion

            return new AutofacServiceProvider(applicationContainer);//新容器
        }

        /// <summary>
        /// 此方法由运行时调用。使用此方法配置HTTP请求管道。
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //使用NLog作为日志记录工具
            loggerFactory.AddNLog();
            //引入Nlog配置文件
            env.ConfigureNLog("Nlog.config");  
            // global cors policy
            app.UseCors(x => x
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                             {
                                 c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                             });
            #endregion
            // ===== Use Authentication ======
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
