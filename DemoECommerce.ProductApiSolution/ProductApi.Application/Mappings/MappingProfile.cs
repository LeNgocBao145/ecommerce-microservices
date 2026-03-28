using AutoMapper;
using ProductApi.Application.DTOs;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductRequestDTO>();
            CreateMap<ProductRequestDTO, Product>();

            CreateMap<Product, ProductResponseDTO>();
            CreateMap<ProductResponseDTO, Product>();
        }
    }
}
