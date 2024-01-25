using AutoMapper;
using POCAutomapperPolymorphic.Dtos;
using POCAutomapperPolymorphic.Models;

namespace POCAutomapperPolymorphic.Profiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<BaseModel, BaseDto>().ReverseMap();
        CreateMap<InnerModel, InnerDto>().ReverseMap();
        CreateMap<DerivedBaseModel, DerivedBaseDto>()
            .Include<DerivedAModel, DerivedADto>()
            .Include<DerivedBModel, DerivedBDto>()
            .ReverseMap();
        CreateMap<DerivedAModel, DerivedADto>().ReverseMap();
        CreateMap<DerivedBModel, DerivedBDto>().ReverseMap();
    }
}
