using System.Collections.Generic;
using System.Threading.Tasks;
using API.Extensions;
using API.Helpers;
using Application.Dtos;
using Application.Interfaces;
using Application.Models;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
   // [ApiController]
  //  [Route("[controller]")]
    public class ProductsController : BaseApiController
    {
        private readonly IProductRepository _productRepository;
        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;

        }


        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
        // {
        //     var products = await _productRepository.GetAll();
        //     return Ok(products);
        // }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> Get(string id)
        {
            var product = await _productRepository.Get(id);
            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            var products = await _productRepository.ProductInquiry(productParams);
        //    Response.AddPagination(products.CurrentPage, products.PageSize,
        //        products.TotalCount, products.TotalPages);
            return Ok(products);
            //return Ok(new Pagination<ProductDto>(products.CurrentPage, products.PageSize, products.TotalCount, products));
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post([FromBody] Product product)
        {

            await _productRepository.Add(product);

            return Ok(product);
        }

        [HttpPut]
        public async Task<ActionResult<Product>> Put([FromBody] Product product)
        {

            await _productRepository.Update(product);

            return Ok(product);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _productRepository.Remove(id);


            return Ok();
        }
    }
}