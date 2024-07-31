using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TestApi.Dtos.ProductDtos;
using TestApi.Dtos.UserDtos;
using TestApi.Models.DB;

namespace TestApi
{
    public class AutoMapperProfile : Profile
    {
        
        public AutoMapperProfile()
        {
            CreateMap<User, GetUserDto>();
            CreateMap<Product, GetProductDto>();
        }
        
    }
}