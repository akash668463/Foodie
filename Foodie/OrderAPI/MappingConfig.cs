using AutoMapper;
using OrderAPI.Models;
using OrderAPI.Models.Dtos;

namespace OrderAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<OrderHeaderDto, CartHeaderDto>().ReverseMap();
            CreateMap<CartDetailsDto, OrderDetailsDto>().ReverseMap();
            CreateMap<OrderDetailsDto, CartDetailsDto>().ReverseMap();
            CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
            CreateMap<OrderDetailsDto, OrderDetails>().ReverseMap();
        }
    }
}
