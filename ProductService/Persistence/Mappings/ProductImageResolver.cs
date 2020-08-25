using System.Collections.Generic;
using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Persistence.Mappings
{
    public class ProductImageResolver : IValueResolver<Product, ProductDto, string>
    {
        private readonly IImageRepository _imgRepo;
        public ProductImageResolver(IImageRepository imgRepo)
        {
            _imgRepo = imgRepo;
        }

        public string Resolve(Product source, ProductDto destination,  string destMember, ResolutionContext context)
        {
            var res = _imgRepo.GetImgBase64(source.ImageIds[0]).GetAwaiter().GetResult();
            return "data:image/jpeg;base64," + res;
        }
    }
}