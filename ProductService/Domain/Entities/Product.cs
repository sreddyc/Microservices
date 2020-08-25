using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
        public ProductType ProductType { get; set; }
        //public string ProductTypeId { get; set; }
        public ProductBrand ProductBrand { get; set; }
        //public string ProductBrandId { get; set; }

        public List<ObjectId> ImageIds {get; set;}
        //public ObjectId ImageIds {get; set;}
    }
}