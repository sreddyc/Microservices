using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Application.Interfaces;
using Persistence.DbContext;
using Domain.Entities;
using Domain.Common;
using Application.Models;
using Application.Helpers;
using Persistence.Extensions;
using Application.Dtos;
using AutoMapper;
using MessagePublish.Sender;
using MongoDB.Bson;

namespace Persistence.Services
{
    public class ProductRepository : IProductRepository
    {

        private readonly MongoContext _context = null;
        private readonly ILoggerManager _logger;

        private readonly IMongoSettings _settings;

        private readonly IImageRepository _imageRepo;

        private readonly IMapper _mapper;

        private readonly IProductUpdateSender _productUpdateSender;

       // private readonly IProductRepository _prodBradRepo;

        public ProductRepository(IMongoSettings settings, ILoggerManager logger, MongoContext context, IImageRepository imageRepo, IMapper mapper, IProductUpdateSender productUpdateSender)
        //, IProductRepository prodBradRepo)
        {
            _settings = settings;
            // _context = new MongoContext(settings);
            _context = context;
            _logger = logger;
            _imageRepo = imageRepo;
            _mapper = mapper;
            _productUpdateSender = productUpdateSender;
           // _prodBradRepo = prodBradRepo;
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            try
            {
                var products = await _context.Products.Find(_ => true).ToListAsync();
                //var productsToReturn = _mapper.Map<IEnumerable<ProductDto>>(products);
                return _mapper.Map<List<Product>, List<ProductDto>>(products);
                // var productsDto = new List<ProductDto>();
                // foreach (var item in products)
                // {
                //     var productDto = new ProductDto
                //     {
                //         Id = item.Id,
                //         Name = item.Name,
                //         Description = item.Description,
                //         Price = item.Price,
                //         ProductType = item.ProductTypeId,
                //         Images = await _imageRepo.DownloadFilesBytes(item.Id)
                //     };
                //     productsDto.Add(productDto);

                // }
            }
            catch (Exception ex)
            {
                // log or manage the exception
                _logger.LogError(String.Format("ProductRepository-GetAll file:{0} {1}", System.Environment.NewLine, ex.ToString()));
                throw ex;
            }
        }

        public async Task<ProductDto> Get(string id)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq("_id", id.ToObjectId());
                var results =  await _context.Products.Find(filter).FirstOrDefaultAsync();
                return _mapper.Map<Product, ProductDto>(results);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                _logger.LogError(String.Format("ProductRepository-Get file:{0} {1}", System.Environment.NewLine, ex.ToString()));
                throw ex;
            }
        }

        public virtual async Task<IQueryable<Product>> GetAllQuery()
        {
            try
            {
                var res = await _context.Products.FindAsync(_ => true);
                return res.ToList().AsQueryable();
            //    var collection = _context._database.GetCollection<Product>("Product");
            //    var result = collection.Aggregate()
            //            .Lookup("Image", "_id", "ProductId", "images")
            //            .ToList().AsQueryable();
            //    return result;
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
            var count = await _context.Products.CountDocumentsAsync(Builders<Product>.Filter.Empty);
            return count;
        }

        public virtual async Task AddAll(IEnumerable<Product> products)
        {
            await _context.Products.InsertManyAsync(products);
        }


        // public async Task<PagedList<Product>> ProductInquiry(ProductInquiryParams productInquiryParams) 
        // {
        //     string sortExpression = productInquiryParams.SortExpression;
		// 	string sortDirection = productInquiryParams.SortDirection;

		// 	int sortOrder = 1;
		// 	if (sortDirection == "desc")
		// 	{
		// 		sortOrder = -1;
		// 	}

		// 	if (productInquiryParams.CurrentPageNumber > 0)
		// 	{
		// 		productInquiryParams.CurrentPageNumber = productInquiryParams.CurrentPageNumber - 1;
		// 	}

        //     //Func<Product, Object> orderByFunc = null;
		// 	if (string.IsNullOrEmpty(sortExpression))
		// 	{
		// 		sortExpression = "{Name: 1}";
        //      //   orderByFunc = product => product.Description;
		// 	}
		// 	else
		// 	{
		// 		sortExpression = "{" + sortExpression + ": " + sortOrder + "}";
		// 	}

        //     //productInquiryParams.ProductNumber = productInquiryParams.ProductNumber.Trim();
		// 	//productInquiryParams.Description = productInquiryParams.Description.Trim();
        //    //_context.GetCollection<Product>("Product").

        //     //var 
        //     IQueryable<Product> query = await GetAllQuery();
        //     // _context.GetCollection<Product>("Product").AsQueryable();
        //     query = query.OrderByDynamic("Description", true);
        //    /* 
        //           query =  (from p in query.AsQueryable()
        //                           orderby p.Id
        //                           select p);
        //                           */
            

        //      if(!string.IsNullOrEmpty(productInquiryParams.ProductNumber))
        //      {
        //        //  Guid guid;
        //        //  Guid.TryParse(productInquiryParams.ProductNumber, out guid);
        //          query = query.Where(x => x.Id == productInquiryParams.ProductNumber);
        //      }

        //      if(!string.IsNullOrEmpty(productInquiryParams.Description))
        //      {
        //         query =  query.Where(x => x.Description == productInquiryParams.Description);
        //      }

             
        //      //var count = CountAsync();
        //     //var query = coll.AsQueryable();
        //     return await PagedList<Product>.CreateAsync(query, productInquiryParams.CurrentPageNumber, productInquiryParams.PageSize);
        // }
        public async Task<Pagination<ProductDto>> ProductInquiry(ProductSpecParams productParams)
        {
            try
            {
                // if (productInquiryParams.PageIndex > 0)
                // {
                //     productInquiryParams.PageIndex = productInquiryParams.PageIndex - 1;
                // }

                FilterDefinition<Product> filter = FilterDefinition<Product>.Empty;
                if (!string.IsNullOrEmpty(productParams.BrandId))
                {
                    filter = Builders<Product>.Filter.Eq(x => x.ProductBrand.Id, productParams.BrandId.ToObjectId());
                }

                if (!string.IsNullOrEmpty(productParams.TypeId))
                {
                    filter = filter & Builders<Product>.Filter.Eq(x => x.ProductType.Id, productParams.TypeId.ToObjectId());
                }

                if (!string.IsNullOrEmpty(productParams.Search))
                {
                    //var regex = new BsonRegularExpression(productParams.Search +"$");
                    //var query = Query<Product>.Matches(p => p.Item, regex);
                   filter = filter & Builders<Product>.Filter.Regex(x => x.Name,  new BsonRegularExpression(productParams.Search,"i"));
                   //filter = filter & Builders<Product>.Filter.AnyEq("Name",  new BsonRegularExpression(productParams.Search));
                }


                SortDefinition<Product> sort = null;

                switch (productParams.Sort)
                {
                    case "priceAsc":
                        sort = Builders<Product>.Sort.Ascending("Price");
                        break;
                    case "priceDesc":
                        sort = Builders<Product>.Sort.Descending("Price");
                        break;
                    default:
                        sort = Builders<Product>.Sort.Ascending("Name");
                        break;
                }

                // if (!string.IsNullOrEmpty(productParams.Sort))
                // {
                //     if (productParams.Sort == "priceDesc")
                //         sort = Builders<Product>.Sort.Descending(sortExpression);
                //     else
                //         sort = Builders<Product>.Sort.Ascending(sortExpression);
                // }

                var filteredQuery = _context.Products.Find(filter).Sort(sort);
                var totalFetch = await filteredQuery.ToListAsync();
                var count = totalFetch.Count();

                var results = await filteredQuery.Skip((productParams.PageSize) * (productParams.PageIndex - 1))
                                    .Limit(productParams.PageSize).ToListAsync();

               // var prodBrands = await _prodBradRepo.GetAll();
                var productDtoToReturn = _mapper.Map<List<Product>, List<ProductDto>>(results);
                // var images = new List<byte[]>();
                // var productDtoToReturn = new List<ProductDto>();
                // foreach (var item in results)
                // {
                //     foreach (var img in item.ImageIds)
                //     {
                //         var imageByte = await _imageRepo.Get(img);
                //         images.Add(imageByte.ImageContent);    
                //     }

                //      var productDto = new ProductDto
                //         {
                //             Name = item.Name,
                //             Description = item.Description,
                //             Price = item.Price,
                //            // ProductTypeId = item.ProductTypeId,
                //            // ProductBrandId = item.ProductBrandId,
                //             Images = images
                //         };
                //     productDtoToReturn.Add(productDto);
                //     //var images = await _imageRepo.Get(item.ImageIds);
                //   //   var imgfilter = Builders<Image>.Filter.Eq("_id", item.ImageIds);
                //   //   await _context.Images.Find(imgfilter).FirstOrDefaultAsync();
                // }
                 _logger.LogInfo("inside paged results");
                 return new Pagination<ProductDto>(productParams.PageIndex, productParams.PageSize, count, productDtoToReturn);
              //  return new PagedList<ProductDto>(productDtoToReturn, results.Count, productParams.PageIndex,
               //                                       productParams.PageSize);
                //return await PagedList<Product>.CreateAsync(filteredQuery, productInquiryParams.CurrentPageNumber,
                //                                      productInquiryParams.PageSize);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                _logger.LogError(String.Format("ProductRepository-ProductInquiry file:{0} {1}", System.Environment.NewLine, ex.ToString()));
                throw ex;
            }

        }

        public async Task<Product> Add(Product product)
        {
            try
            {
                await _context.Products.InsertOneAsync(product);
              //  _productUpdateSender.Publish( product);
                return product;
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
                DeleteResult actionResult = await _context.Products.DeleteOneAsync(
                     Builders<Product>.Filter.Eq("Id", id));

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


        public async Task<bool> Update(Product product)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(s => s.Id, product.Id);
                var update = Builders<Product>.Update
                        //   .Set(p => p.ProductNumber, item.ProductNumber)
                        .Set(p => p.Description, product.Description);
                //.Set(p => p.UnitPrice, item.UnitPrice);
                UpdateResult actionResult = await _context.Products.UpdateOneAsync(filter, update);

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