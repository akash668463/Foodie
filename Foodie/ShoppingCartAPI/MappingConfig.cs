using AutoMapper;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Models.Dtos;

namespace ShoppingCartAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
            CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
        }
    }
}