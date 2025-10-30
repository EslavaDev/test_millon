using AutoMapper;
using RealEstate.Application.DTOs;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.Services;

public class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public PropertyService(IPropertyRepository propertyRepository, IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<PropertyListDto>> GetPropertiesAsync(PropertyFilterDto filterDto)
    {
        // Map DTO to domain filter
        var domainFilter = _mapper.Map<PropertyFilter>(filterDto);

        // Call repository
        var (properties, totalCount) = await _propertyRepository.GetFilteredAsync(domainFilter);

        // Map results to DTOs
        var propertyDtos = _mapper.Map<List<PropertyListDto>>(properties);

        // Create paged result
        return new PagedResultDto<PropertyListDto>
        {
            Items = propertyDtos,
            PageNumber = filterDto.PageNumber,
            PageSize = filterDto.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PropertyDetailDto?> GetPropertyByIdAsync(string id)
    {
        var property = await _propertyRepository.GetByIdAsync(id);

        if (property == null)
        {
            return null;
        }

        return _mapper.Map<PropertyDetailDto>(property);
    }
}
