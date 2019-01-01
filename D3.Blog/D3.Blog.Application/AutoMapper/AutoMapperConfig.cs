using AutoMapper;

namespace D3.Blog.Application.AutoMapper
{
    /// <summary>
    /// AutoMapper配置文件
    /// </summary>
    public class AutoMapperConfig
    {
        /// <summary>
        /// 配置automapper
        /// </summary>
        /// <returns></returns>
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
                                           {
                                               cfg.AddProfile(new DomainToViewModelMappingProfile());
                                               cfg.AddProfile(new ViewModelToDomainMappingProfile());
                                           });
        }
    }
}
