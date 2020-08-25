using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProductBrandRepository
    {
        Task<IEnumerable<ProductBrandDto>> GetAll();

         Task<IEnumerable<ProductBrand>> GetAllBrands();
        Task<ProductBrand> Get(string id);

        Task Add(ProductBrand productType);

        Task<bool> Update(ProductBrand productType);

        Task<bool> Remove(string id);

        Task<long> CountAsync();

        Task AddAll(IEnumerable<ProductBrand> productTypes);

        Task<ProductBrand> GetByName(string brandName);
    }
}