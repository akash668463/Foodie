using AutoMapper;
using ProductAPI.Models;
using ProductAPI.Models.Dtos;

namespace ProductAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
        }
    }
}