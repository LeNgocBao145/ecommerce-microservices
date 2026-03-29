using AutoMapper;
using OrderApi.Application.DTOs;
using OrderApi.Domain.Entities;

namespace OrderApi.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderResponseDTO>();
            CreateMap<OrderResponseDTO, Order>();
        }
    }
}
