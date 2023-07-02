namespace BusinessLogicLayer.AutoMapper
{
    using global::AutoMapper;

    public abstract class AutoMapperService : IAutoMapperService
    {
        public IMapper Mapper
        {
            get { return ObjectMapper.Mapper; }
        }
    }
}
