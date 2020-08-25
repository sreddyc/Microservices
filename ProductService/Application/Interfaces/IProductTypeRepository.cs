using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Helpers;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProductTypeRepository
    {
        Task<IEnumerable<ProductTypeDto>> GetAll();

         Task<IEnumerable<ProductType>> GetAllTypes();
        Task<ProductType> Get(string id);

        Task Add(ProductType productType);

        Task<bool> Update(ProductType productType);

        Task<bool> Remove(string id);

        Task<long> CountAsync();

        Task AddAll(IEnumerable<ProductType> productTypes);

        Task<ProductType> GetByName(string typeName);
    }
}