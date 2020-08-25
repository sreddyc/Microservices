using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductTypesController : BaseApiController
    {
          private readonly IProductTypeRepository _productTypeRepository;
        public ProductTypesController(IProductTypeRepository productTypeRepository)
        {
            _productTypeRepository = productTypeRepository;

        }


        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<ProductTypeDto>>> GetProductTypes()
        {
            var productTypes = await _productTypeRepository.GetAll();
            return Ok(productTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductType>> Get(string id)
        {
            var product = await _productTypeRepository.Get(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post([FromBody] ProductType productType)
        {

            await _productTypeRepository.Add(productType);

            return Ok(productType);
        }

        [HttpPut]
        public async Task<ActionResult<Product>> Put([FromBody] ProductType productType)
        {

            await _productTypeRepository.Update(productType);

            return Ok(productType);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _productTypeRepository.Remove(id);


            return Ok();
        }
    }
}