using AutoMapper;

namespace ProjectsApp.Classes.Helpers
{
    public class MapperHelper
    {
        public static IMapper CreateMap<T1, T2>()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<T1, T2>());
            var mapper = config.CreateMapper();
            return new MappingEngine(config, mapper).Mapper;
        }
    }
}