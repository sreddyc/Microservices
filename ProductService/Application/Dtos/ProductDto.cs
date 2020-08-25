using System.Collections.Generic;
using Domain.Entities;

namespace Application.Dtos
{
    public class ProductDto
    {
        public string Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string ProductType { get; set; }
        public string ProductBrand { get; set; }
        
    }
}