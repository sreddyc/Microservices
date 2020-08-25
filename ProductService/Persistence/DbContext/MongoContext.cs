using System;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using MongoDB.Driver;

namespace Persistence.DbContext
{
    public class MongoContext
    { 
        public readonly IMongoDatabase _database = null;
        private readonly IMongoSettings _settings;

        public MongoContext(IMongoSettings settings)
        {
            _settings = settings;
            var client = new MongoClient(_settings.Connection);
            if (client != null)
                _database = client.GetDatabase(_settings.DatabaseName);
        }

        public IMongoCollection<Product> Products
        {
            get
            {
                return _database.GetCollection<Product>("Product");
            }
        }

        public IMongoCollection<Image> Images
        {
            get
            {
                return _database.GetCollection<Image>("Image");
            }
        }

        public IMongoCollection<ProductType> ProductTypes
        {
            get
            {
                return _database.GetCollection<ProductType>("ProductType");
            }
        }

        public IMongoCollection<ProductBrand> ProductBrands
        {
            get
            {
                return _database.GetCollection<ProductBrand>("ProductBrand");
            }
        }
        

    }
}