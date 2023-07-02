namespace BusinessLogicLayer.AutoMapper
{
    using global::AutoMapper;

    public interface IAutoMapperService
    {
        IMapper Mapper { get; }
    }
}
