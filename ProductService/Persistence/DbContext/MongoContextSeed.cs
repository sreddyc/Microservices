using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using CsvHelper;
using Domain.Common;
using Domain.Entities;
using MongoDB.Bson;

namespace Persistence.DbContext
{
    public class MongoContextSeed
    {
        IProductRepository _productRepo;
        IProductBrandRepository _productBrandRepo;

        IProductTypeRepository _productTypeRepo;

        IImageRepository _imageRepo;
        ILoggerManager _logger;
        IMapper _mapper;
        public MongoContextSeed(IProductRepository productRepo, IProductBrandRepository productBrandRepo, IProductTypeRepository productTypeRepo,
                                IImageRepository imageRepo, ILoggerManager logger, IMapper mapper)
        {
            _productRepo = productRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
            _logger = logger;
            _mapper = mapper;
            _imageRepo = imageRepo;


        }

        // public FormFile ConvertToForm(string FilePath)
        // {

        //     //using (var stream = File.OpenRead(@FilePath))
        // 	//{
        // 		var stream = File.OpenRead(@FilePath);
        //         FormFile file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
        //         {
        //             Headers = new HeaderDictionary(),
        //             ContentType = FilePath.GetContentType()
        //         };
        // 		stream.Position =0;
        // 		return file;
        //     //}

        // }

        public async Task SeedAsync()
        {
            try
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (await _productBrandRepo.CountAsync() == 0)
                {
                    var brandsData =
                        File.ReadAllText(path + @"/SeedData/brands.json");

                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    foreach (var item in brands)
                    {
                        await _productBrandRepo.Add(item);
                    }

                }

                if (await _productTypeRepo.CountAsync() == 0)
                {
                    var typesData =
                        File.ReadAllText(path + @"/SeedData/types.json");

                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    foreach (var item in types)
                    {
                        await _productTypeRepo.Add(item);
                    }

                }
                await SeedProducts();

                if (await _productRepo.CountAsync() == 0)
                {
                    var productsData =
                        File.ReadAllText(path + @"/SeedData/products1.json");

                    var products = JsonSerializer.Deserialize<List<ProductSeed>>(productsData);

                    foreach (var item in products)
                    {
                        var images = new List<ObjectId>();
                        foreach (var url in item.PictureUrls)
                        {
                            var image = new Image
                            {
                                // ProductId = createdProduct.Id,
                                ImageContent = File.ReadAllBytes(path + url)
                            };
                            // var byteFile = File.ReadAllBytes(path + url);
                            await _imageRepo.Upload(image);
                            images.Add(image.Id);

                        }
                        var product = new Product
                        {
                            Name = item.Name,
                            Description = item.Description,
                            Price = item.Price,
                            ProductBrand = await _productBrandRepo.GetByName("NetCore"),
                            ProductType = await _productTypeRepo.GetByName("Hats"),
                            //  ProductType = item.ProductType,
                            //  ProductBrand = item.ProductBrand,
                            ImageIds = images
                        };
                        var createdProduct = await _productRepo.Add(product);


                    }

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        static Random rndBrand = new Random();
        static Random rndType = new Random();
        static Random rndImage = new Random();

        private ProductBrand GetRandomBrand()
        {
            var brands = _productBrandRepo.GetAllBrands().GetAwaiter().GetResult();
            var brandsList = brands.ToList();
            int index = rndBrand.Next(brandsList.Count());
            return brandsList[index];
        }

        private ProductType GetRandomType()
        {
            var types = _productTypeRepo.GetAllTypes().GetAwaiter().GetResult();
            var typesList = types.ToList();
            int index = rndType.Next(typesList.Count());
            return typesList[index];
        }

        private string GetRandomImage()
        {
            var images = new List<string>() { "/SeedData/images/products/beer.jfif",
                    "/SeedData/images/products/scot.jfif",
                    "/SeedData/images/products/vod.jfif",
                    "/SeedData/images/products/wine.jfif"};
            int index = rndImage.Next(images.Count());
            return images[index];
        }
        public async Task SeedProducts()
        {
            try
            {
                if (await _productRepo.CountAsync() > 0)
                    return;



                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                using (var reader = new StreamReader(path + @"\SeedData\products.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<ProductSeed>();
                    int price  = 0;
                    foreach (var item in records)
                    {
                        price++;
                        var images = new List<ObjectId>();
                     //   foreach (var url in item.PictureUrls)
                     //   {
                            var image = new Image
                            {
                                // ProductId = createdProduct.Id,
                                ImageContent = File.ReadAllBytes(path + GetRandomImage())
                            };
                            // var byteFile = File.ReadAllBytes(path + url);
                            await _imageRepo.Upload(image);
                            images.Add(image.Id);

                       // }
                        var product = new Product
                        {
                            Name = item.Name,
                            Description = item.Description,
                            Price =  price, //item.Price,
                            ProductBrand = GetRandomBrand(),
                            ProductType = GetRandomType(),
                            //  ProductType = item.ProductType,
                            //  ProductBrand = item.ProductBrand,
                            ImageIds = images
                        };
                        var createdProduct = await _productRepo.Add(product);

                    }
                    // var products = _mapper.Map<IEnumerable<Product>>(records);
                    // _productRepository.AddAll(products);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(String.Format("SeedProducts:{0} {1}", System.Environment.NewLine, ex.ToString()));
            }
        }
    }
}