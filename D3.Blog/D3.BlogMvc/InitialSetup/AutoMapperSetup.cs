using System;
using Autofac;
using AutoMapper;
using D3.Blog.Application.AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace D3.BlogMvc.InitialSetup
{
    public static class AutoMapperSetup
    {
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
