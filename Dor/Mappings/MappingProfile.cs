using AutoMapper;
using Dor.Dtos.Building;
using Dor.Dtos.Complex;
using Dor.Dtos.Customer;
using Dor.Dtos.Property;
using Dor.Models;

namespace Dor.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Building Mappings
        CreateMap<Building, BuildingDto>();
        CreateMap<CreateBuildingDto, Building>();
        CreateMap<UpdateBuildingDto, Building>();

        // Complex Mappings
        CreateMap<Complex, ComplexDto>();
        CreateMap<CreateComplexDto, Complex>();
        CreateMap<UpdateComplexDto, Complex>();

        // Customer Mappings
        CreateMap<Customers, CustomerDto>();
        CreateMap<CreateCustomerDto, Customers>();
        CreateMap<UpdateCustomerDto, Customers>();

        // Property Mappings
        CreateMap<Property, PropertyDto>();
        CreateMap<CreatePropertyDto, Property>();
        CreateMap<UpdatePropertyDto, Property>();
    }
}