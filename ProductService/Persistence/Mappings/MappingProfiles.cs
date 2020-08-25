using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Persistence.Extensions;
using Persistence.Mappings;
using System;
using System.Linq;

namespace Infrastructure.Helpers
{
   public class MappingProfiles : Profile
    {
       
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>() 
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductImageResolver>()).ReverseMap();
            //.ForMember(c => c.Id, option => option.Ignore());
         //   .ForMember(d => d.Images, map => map.MapFrom((s,d) => s.Id.GetImagesForProduct())).ReverseMap();
           //  .ForMember(dest => dest.Images, opt => {
           //         opt.ResolveUsing(d => d.Id.CalculateAge());
           //     });
            //    .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
            //    .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name));
                //.ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
             CreateMap<ProductBrand, ProductBrandDto>().ReverseMap();//.ForMember(c => c.Id, option => option.Ignore());
             CreateMap<ProductType, ProductTypeDto>().ReverseMap();//.ForMember(c => c.Id, option => option.Ignore());
        }
    }
}