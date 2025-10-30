using AutoMapper;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // PropertyFilterDto to domain PropertyFilter
        CreateMap<PropertyFilterDto, PropertyFilter>();

        // Owner mappings
        CreateMap<Owner, OwnerBasicDto>();
        CreateMap<Owner, OwnerDto>();

        // PropertyImage mappings
        CreateMap<PropertyImage, PropertyImageDto>();

        // PropertyTrace mappings
        CreateMap<PropertyTrace, PropertyTraceDto>();

        // Property to PropertyListDto
        CreateMap<Property, PropertyListDto>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src =>
                src.Images.FirstOrDefault(img => img.Enabled) != null
                    ? src.Images.FirstOrDefault(img => img.Enabled)!.File
                    : null));

        // Property to PropertyDetailDto
        CreateMap<Property, PropertyDetailDto>();
    }
}
