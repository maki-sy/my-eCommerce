using AutoMapper;
using BusinessObject.Models;
using eStoreAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace eStoreAPI.MapperProfiles
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(des => des.ProductName,
                act => act.MapFrom(src => src.ProductName))
                .ForMember(des => des.CategoryId,
                act => act.MapFrom(src => src.CategoryId))
                .ForMember(des => des.UnitsInStock,
                act => act.MapFrom(src => src.UnitsInStock))
                .ForMember(des => des.UnitPrice,
                act => act.MapFrom(src => src.UnitPrice));
            CreateMap<ProductDTO, Product>();

            CreateMap<Member, MemberDTO>()
                .ForMember(des => des.Email,
                act => act.MapFrom(src => src.Email))
                .ForMember(des => des.CompanyName,
                act => act.MapFrom(src => src.CompanyName))
                .ForMember(des => des.City,
                act => act.MapFrom(src => src.City))
                .ForMember(des => des.Country,
                act => act.MapFrom(src => src.Country))
                .ForMember(des => des.Password,
                act => act.MapFrom(src => src.Password));
            CreateMap<MemberDTO, Member>();

            CreateMap<Member, LoginDTO>()
                .ForMember(des => des.Email,
                act => act.MapFrom(src => src.Email))
                .ForMember(des => des.Password,
                act => act.MapFrom(src => src.Password));
            CreateMap<LoginDTO, Member>();

            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
        }
    }
}
