using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductBrandsController : BaseApiController
    {
         private readonly IProductBrandRepository _productBrandRepository;
        public ProductBrandsController(IProductBrandRepository productBrandRepository)
        {
            _productBrandRepository = productBrandRepository;

        }


        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<ProductBrandDto>>> GetProductBrands()
        {
            var productBrands = await _productBrandRepository.GetAll();
            return Ok(productBrands);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductBrand>> Get(string id)
        {
            var product = await _productBrandRepository.Get(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductBrand>> Post([FromBody] ProductBrand productBrand)
        {

            await _productBrandRepository.Add(productBrand);

            return Ok(productBrand);
        }

        [HttpPut]
        public async Task<ActionResult<ProductBrand>> Put([FromBody] ProductBrand productBrand)
        {

            await _productBrandRepository.Update(productBrand);

            return Ok(productBrand);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _productBrandRepository.Remove(id);


            return Ok();
        }
        
    }
}