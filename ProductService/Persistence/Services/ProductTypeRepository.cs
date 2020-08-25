using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using MongoDB.Driver;
using Persistence.DbContext;


namespace Persistence.Services
{
    public class ProductTypeRepository : IProductTypeRepository
    {
         private readonly MongoContext _context = null;
        private readonly ILoggerManager _logger;

        private readonly IMongoSettings _settings;

        private readonly IMapper _mapper;

        public ProductTypeRepository(IMongoSettings settings, ILoggerManager logger, MongoContext context, IMapper mapper)
        {
            _settings = settings;
           // _context = new MongoContext(settings);
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductTypeDto>> GetAll()
        {
            try
            {
                var types = await _context.ProductTypes.Find(_ => true).ToListAsync();
                 var productTypesToReturn = _mapper.Map<List<ProductType>, List<ProductTypeDto>>(types);
                return productTypesToReturn;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                _logger.LogError(String.Format("ProductRepository-GetAll file:{0} {1}", System.Environment.NewLine, ex.ToString()));
                throw ex;
            }
        }

        public async Task<IEnumerable<ProductType>> GetAllTypes()
        {
            try
            {
                var types = await _context.ProductTypes.Find(_ => true).ToListAsync();
                return types;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                _logger.LogError(String.Format("ProductRepository-GetAll file:{0} {1}", System.Environment.NewLine, ex.ToString()));
                throw ex;
            }
        }

        public async Task<ProductType> Get(string id)
        {
            try
            {
               var filter = Builders<ProductType>.Filter.Eq("_id", id);
                return await _context.ProductTypes.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                _logger.LogError(String.Format("ProductRepository-Get file:{0} {1}", System.Environment.NewLine, ex.ToString()));
                throw ex;
            }
        }

        public async Task<ProductType> GetByName(string typeName)
        {
            try
            {
                var filter = Builders<ProductType>.Filter.Eq("Name", typeName);
                return await _context.ProductTypes.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                _logger.LogError(String.Format("ProductRepository-Get file:{0} {1}", System.Environment.NewLine, ex.ToString()));
                throw ex;
            }
        }


        public virtual async Task<IQueryable<ProductType>> GetAllQuery()
        {
            try
            {
                var res = await _context.ProductTypes.FindAsync(_ => true);
                return res.ToList().AsQueryable();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                _logger.LogError(String.Format("ProductRepository-GetAllQuery file:{0} {1}", System.Environment.NewLine, ex.ToString()));
                throw ex;
            }
        }

        public virtual async Task<long> CountAsync()
        {
            var count = await _context.ProductTypes.CountDocumentsAsync(Builders<ProductType>.Filter.Empty);
            return count;
        }

        public virtual async Task AddAll(IEnumerable<ProductType> productTypes)
        {
              await _context.ProductTypes.InsertManyAsync(productTypes);
        }

        public async Task Add(ProductType productType)
        {
            try
            {
                await _context.ProductTypes.InsertOneAsync(productType);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                _logger.LogError(String.Format("ProductRepository-Add file:{0} {1}", System.Environment.NewLine, ex.ToString()));
                throw ex;
            }
        }

        public async Task<bool> Remove(string id)
        {
            try
            {
                DeleteResult actionResult = await _context.ProductTypes.DeleteOneAsync(
                     Builders<ProductType>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                _logger.LogError(String.Format("ProductRepository-Remove file:{0} {1}", System.Environment.NewLine, ex.ToString()));
                throw ex;
            }
        }


        public async Task<bool> Update(ProductType productType)
        {
            try
            {
                var filter = Builders<ProductType>.Filter.Eq(s => s.Id, productType.Id);
                var update = Builders<ProductType>.Update
                     //   .Set(p => p.ProductNumber, item.ProductNumber)
                        .Set(p => p.Name, productType.Name);
                        //.Set(p => p.UnitPrice, item.UnitPrice);
                UpdateResult actionResult = await _context.ProductTypes.UpdateOneAsync(filter, update);

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;

            }
            catch (Exception ex)
            {
                // log or manage the exception
                _logger.LogError(String.Format("ProductRepository-Update file:{0} {1}", System.Environment.NewLine, ex.ToString()));
                throw ex;
            }
        }
    }
}