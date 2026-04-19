using AutoMapper;
using OrderAPI.Models;
using OrderAPI.Models.Dtos;

namespace OrderAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            // map cart/header and order/header both ways
            CreateMap<CartHeaderDto, OrderHeaderDto>().ReverseMap();

            // ensure price & product name are copied from nested Product to OrderDetails.Price/ProductName
            CreateMap<CartDetailsDto, OrderDetailsDto>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product != null ? src.Product.Price : 0.0))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : src.Product.Name))
                .ReverseMap();

            CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
            CreateMap<OrderDetailsDto, OrderDetails>().ReverseMap();
        }
    }
}
