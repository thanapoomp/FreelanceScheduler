using AutoMapper;
using freelance.api.Models;
using freelance.api.Dtos;

namespace freelance.api.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForReturnDto>();
            CreateMap<Customer, CustomerForCreateDto>().ReverseMap();
            CreateMap<Customer, CustomerForUpdateDto>().ReverseMap();
            CreateMap<Product, ProductForCreateDto>().ReverseMap();
            CreateMap<Product, ProductForUpdateDto>().ReverseMap();
        }
    }
}