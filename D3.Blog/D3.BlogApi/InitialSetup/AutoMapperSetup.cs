using System;
using AutoMapper;
using D3.Blog.Application.AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace D3.BlogApi.InitialSetup
{
    /// <summary>
    /// automap配置
    /// </summary>
    public static class AutoMapperSetup
    {
        /// <summary>
        /// 注入automapper
        /// </summary>
        /// <param name="services"></param>
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper();

            // Registering Mappings automatically only works if the 
            // Automapper Profile classes are in ASP.NET project
            AutoMapperConfig.RegisterMappings();
        }
    }
}
