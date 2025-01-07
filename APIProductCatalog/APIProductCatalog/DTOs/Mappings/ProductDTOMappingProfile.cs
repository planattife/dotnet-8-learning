using APIProductCatalog.Models;
using AutoMapper;

namespace APIProductCatalog.DTOs.Mappings;

public class ProductDTOMappingProfile : Profile
{
    public ProductDTOMappingProfile()
    {
        CreateMap<Product, ProductDTO>().ReverseMap();
    }
}
