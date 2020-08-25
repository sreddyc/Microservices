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
    public class ProductBrandRepository : IProductBrandRepository
    {
         private readonly MongoContext _context = null;
        private readonly ILoggerManager _logger;

        private readonly IMongoSettings _settings;

        private readonly IMapper _mapper;

        public ProductBrandRepository(IMongoSettings settings, ILoggerManager logger, MongoContext context, IMapper mapper)
        {
            _settings = settings;
            //_context = new MongoContext(settings);
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductBrandDto>> GetAll()
        {
            try
            {
                var brands =  await _context.ProductBrands.Find(_ => true).ToListAsync();
                var productBrandsToReturn = _mapper.Map<List<ProductBrand>, List<ProductBrandDto>>(brands);
                return productBrandsToReturn;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                _logger.LogError(string.Format("ProductRepository-GetAll file:{0} {1}", System.Environment.NewLine, ex.ToString()));
                throw ex;
            }
        }

        public async Task<IEnumerable<ProductBrand>> GetAllBrands()
        {
            try
            {
                var brands =  await _context.ProductBrands.Find(_ => true).ToListAsync();
                return brands;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                _logger.LogError(string.Format("ProductRepository-GetAll file:{0} {1}", System.Environment.NewLine, ex.ToString()));
                throw ex;
            }
        }

        public async Task<ProductBrand> Get(string id)
        {
            try
            {
               var filter = Builders<ProductBrand>.Filter.Eq("_id", id);
                return await _context.ProductBrands.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                _logger.LogError(String.Format("ProductRepository-Get file:{0} {1}", System.Environment.NewLine, ex.ToString()));
                throw ex;
            }
        }

        public async Task<ProductBrand> GetByName(string brandName)
        {
            try
            {
                var filter = Builders<ProductBrand>.Filter.Eq("Name", brandName);
                return await _context.ProductBrands.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                _logger.LogError(String.Format("ProductRepository-Get file:{0} {1}", System.Environment.NewLine, ex.ToString()));
                throw ex;
            }
        }

        public virtual async Task<IQueryable<ProductBrand>> GetAllQuery()
        {
            try
            {
                var res = await _context.ProductBrands.FindAsync(_ => true);
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
            var count = await _context.ProductBrands.CountDocumentsAsync(Builders<ProductBrand>.Filter.Empty);
            return count;
        }

        public virtual async Task AddAll(IEnumerable<ProductBrand> productBrands)
        {
              await _context.ProductBrands.InsertManyAsync(productBrands);
        }

        public async Task Add(ProductBrand productBrand)
        {
            try
            {
                await _context.ProductBrands.InsertOneAsync(productBrand);
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
                DeleteResult actionResult = await _context.ProductBrands.DeleteOneAsync(
                     Builders<ProductBrand>.Filter.Eq("Id", id));

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


        public async Task<bool> Update(ProductBrand productBrand)
        {
            try
            {
                var filter = Builders<ProductBrand>.Filter.Eq(s => s.Id, productBrand.Id);
                var update = Builders<ProductBrand>.Update
                     //   .Set(p => p.ProductNumber, item.ProductNumber)
                        .Set(p => p.Name, productBrand.Name);
                        //.Set(p => p.UnitPrice, item.UnitPrice);
                UpdateResult actionResult = await _context.ProductBrands.UpdateOneAsync(filter, update);

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