using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Helpers;
using Application.Models;
using Domain.Common;
using Domain.Entities;
using MongoDB.Bson;

namespace Application.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetAll();
        Task<ProductDto> Get(string id);

        Task<Pagination<ProductDto>> ProductInquiry(ProductSpecParams productParams);

        Task<Product> Add(Product product);

        Task<bool> Update(Product product);

        Task<bool> Remove(string id);

        Task<long> CountAsync();

         Task AddAll(IEnumerable<Product> obj);
    }
}