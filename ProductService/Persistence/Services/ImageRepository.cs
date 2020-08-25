using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using Application.Interfaces;
using Persistence.DbContext;
using Domain.Common;
// using MongoDB.Driver.GridFS;
using Domain.Entities;
using Persistence.Extensions;

namespace Persistence.Services
{
    public class ImageRepository : IImageRepository
    {

        private readonly MongoContext _context = null;
        private readonly ILoggerManager _logger;

        public ImageRepository(IMongoSettings settings, ILoggerManager logger)
        {
            _context = new MongoContext(settings);
            _logger = logger;
        }
        public async Task<ObjectId> Upload(Image image)
        {

            try
            {
                await _context.Images.InsertOneAsync(image);
                return image.Id;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                _logger.LogError(String.Format("ImageRepository-Add file:{0} {1}", System.Environment.NewLine, ex.ToString()));
                throw ex;
            }

        }

        public async Task<Image> Get(ObjectId id)
        {
            try
            {
                var filter = Builders<Image>.Filter.Eq("_id", id);
                return await _context.Images.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                _logger.LogError(String.Format("ImageRepository-Get file:{0} {1}", System.Environment.NewLine, ex.ToString()));
                throw ex;
            }
        }

        public async Task<string> GetImgBase64(ObjectId id)
        {
            try
            {
                var filter = Builders<Image>.Filter.Eq("_id", id);
                var image =  await _context.Images.Find(filter).FirstOrDefaultAsync();
                return System.Convert.ToBase64String(image.ImageContent);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                _logger.LogError(String.Format("ImageRepository-Get file:{0} {1}", System.Environment.NewLine, ex.ToString()));
                throw ex;
            }
        }

        public List<byte[]> Get(List<ObjectId> ids)
        {
            try
            {
                var images = new List<byte[]>();
                foreach (var item in ids)
                {
                    var img = Get(item).GetAwaiter().GetResult();
                    images.Add(img.ImageContent);
                }
              //  var filter = Builders<Image>.Filter.AnyEq("_id", ids);
              //  return await _context.Images.Find(filter).ToListAsync();
              return images;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                _logger.LogError(String.Format("ImageRepository-Get file:{0} {1}", System.Environment.NewLine, ex.ToString()));
                throw ex;
            }
        }

        // public async Task<IEnumerable<Image>> Get(List<ObjectId> ids)
        // {
        //     try
        //     {
        //         var filter = Builders<Image>.Filter.AnyEq("_id", ids);
        //         return await _context.Images.Find(filter).ToListAsync();
        //     }
        //     catch (Exception ex)
        //     {
        //         // log or manage the exception
        //         _logger.LogError(String.Format("ImageRepository-Get file:{0} {1}", System.Environment.NewLine, ex.ToString()));
        //         throw ex;
        //     }
        // }

        public async Task<Image> GetByProductId(string id)
        {
            try
            {
                var filter = Builders<Image>.Filter.Eq("ProductId", id);
                return await _context.Images.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                _logger.LogError(String.Format("ImageRepository-Get file:{0} {1}", System.Environment.NewLine, ex.ToString()));
                throw ex;
            }
        }

        // public async Task<string> UploadFile(IFormFile files, string productId)
        // {

        //     try
        //     {
        //         var fileBytes = await GetByteArrayFromImageAsync(files);
        //         var fs = new GridFSBucket(_context._database);
        //         var options = new GridFSUploadOptions
        //         {
        //             // ChunkSizeBytes = 64512, // 63KB
        //             Metadata = new BsonDocument
        //         {
        //             { "productID", productId }
        //         }
        //         };
        //         var t = await Task.Run<ObjectId>(async () =>
        //         {
        //             return await fs.UploadFromBytesAsync(files.FileName, fileBytes, options);
        //         });
        //         var image = new Image
        //         {
        //             FileId = t.ToString(),
        //             ProductId = productId
        //         };
        //         await Add(image);
        //         return t.ToString();
        //     }


        //     catch (Exception ex)
        //     {
        //         _logger.LogError(String.Format("FileUploadRepository-UploadFile file:{0} {1}", System.Environment.NewLine, ex.ToString()));
        //         throw ex;
        //     }


        // }

        // public async Task<string> UploadFile(byte[] fileBytes, string productId)
        // {

        //     try
        //     {
        //         // var fileBytes = await GetByteArrayFromImageAsync(files);
        //         var fs = new GridFSBucket(_context._database);
        //         var options = new GridFSUploadOptions
        //         {
        //             // ChunkSizeBytes = 64512, // 63KB
        //             Metadata = new BsonDocument
        //         {
        //             { "productID", productId }
        //         }
        //         };
        //         var t = await Task.Run<ObjectId>(async () =>
        //         {
        //             return await fs.UploadFromBytesAsync(productId, fileBytes, options);
        //         });
        //         var image = new Image
        //         {
        //             FileId = t.ToString(),
        //             ProductId = productId
        //         };
        //         await Add(image);
        //         return t.ToString();
        //     }


        //     catch (Exception ex)
        //     {
        //         _logger.LogError(String.Format("FileUploadRepository-UploadFile file:{0} {1}", System.Environment.NewLine, ex.ToString()));
        //         throw ex;
        //     }


        // }

        // public async Task Add(Image image)
        // {
        //     try
        //     {
        //         await _context.Images.InsertOneAsync(image);
        //     }
        //     catch (Exception ex)
        //     {
        //         // log or manage the exception
        //         _logger.LogError(String.Format("FileUploadRepository-Add file:{0} {1}", System.Environment.NewLine, ex.ToString()));
        //         throw ex;
        //     }
        // }

        // public virtual async Task<long> CountAsync()
        // {
        //     var count = await _context.Images.CountDocumentsAsync(Builders<Image>.Filter.Empty);
        //     return count;
        // }
        // private async Task<byte[]> GetByteArrayFromImageAsync(IFormFile file)
        // {
        //     using (var target = new MemoryStream())
        //     {
        //         await file.CopyToAsync(target);
        //         return target.ToArray();
        //     }
        // }

        // public async Task<IEnumerable<Image>> GetFileIdByProdId(string id)
        // {
        //     try
        //     {
        //         var filter = Builders<Image>.Filter.Eq("ProductId", id);
        //         return await _context.Images.Find(filter).ToListAsync();

        //     }
        //     catch (Exception ex)
        //     {
        //         // log or manage the exception
        //         _logger.LogError(String.Format("FileRepository-Get file:{0} {1}", System.Environment.NewLine, ex.ToString()));
        //         throw ex;
        //     }
        // }

        // public async Task<bool> Delete(string productNumber)
        // {
        //     try
        //     {
        //         var fs = new GridFSBucket(_context._database);
        //         var images = await GetFileIdByProdId(productNumber);
        //         foreach (var item in images)
        //         {
        //             await fs.DeleteAsync(item.FileId.ToObjectId());
        //         }
        //         DeleteResult actionResult = await _context.Images.DeleteOneAsync(
        //              Builders<Image>.Filter.Eq("ProductNumber", productNumber));
        //         return true;

        //     }
        //     catch (Exception ex)
        //     {
        //         // log or manage the exception
        //         _logger.LogError(String.Format("FileRepository-Get file:{0} {1}", System.Environment.NewLine, ex.ToString()));
        //         throw ex;
        //     }
        // }

        // public async Task<GridFSFileInfo> GetFileInfo(string fileID, GridFSBucket fs)
        // {
        //     try
        //     {
        //         var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", fileID.ToObjectId());

        //         /*
        //        var filter = Builders<GridFSFileInfo>.Filter.And(
        //            Builders<GridFSFileInfo>.Filter.Eq(x => x.Id, fileID.ToObjectId())
        //             );
        //             */
        //         var sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);
        //         var options = new GridFSFindOptions
        //         {
        //             Limit = 1,
        //             Sort = sort
        //         };
        //         using (var cursor = await fs.FindAsync(filter, options))
        //         {
        //             var fileInfo = (await cursor.ToListAsync()).FirstOrDefault();
        //             // fileInfo either has the matching file information or is null
        //             return fileInfo;
        //         }

        //     }
        //     catch (Exception ex)
        //     {
        //         // log or manage the exception
        //         _logger.LogError(String.Format("GetFileInfo-Get file:{0} {1}", System.Environment.NewLine, ex.ToString()));
        //         throw ex;
        //     }
        // }


        // public async Task<List<byte[]>> DownloadFilesBytes(string productNumber)
        // {

        //     try
        //     {
        //         //  var streamImages = new List<ItemsToZip>();
        //         var images = await GetFileIdByProdId(productNumber);

        //         int i = 1;
        //         var bytesToReturn = new List<byte[]>();
        //         foreach (var img in images)
        //         {

        //             var fs = new GridFSBucket(_context._database);
        //             var fileInfo = await GetFileInfo(img.FileId, fs);
        //             var bytes = await fs.DownloadAsBytesAsync(img.FileId.ToObjectId());
        //             bytesToReturn.Add(bytes);
        //             //  var itemsToZip = new ItemsToZip();
        //             //   itemsToZip.Name = string.Format("{0}_{1}{2}", productNumber, i, Path.GetExtension(fileInfo.Filename));
        //             // using (var stream = new MemoryStream(bytes))

        //             //  var stream = new MemoryStream(bytes);

        //             //  itemsToZip.Content = stream;
        //             i++;
        //             //  var bytes = await fs.DownloadAsBytesByNameAsync(@"test.txt");
        //             // await fs.DownloadToStreamByNameAsync(@"test.txt", stream);
        //             //  await fs.DownloadToStreamAsync("5ec43ce8ae36e1683829eeaa", stream);
        //             // await fs.DownloadToStreamAsync(img.FileID.ToObjectId(), stream);
        //             // streamImages.Add(itemsToZip);
        //             // stream.Close();


        //         }
        //         return bytesToReturn;


        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(String.Format("FileRepository-DownloadFile file:{0} {1}", System.Environment.NewLine, ex.ToString()));
        //         throw ex;
        //     }
        //     finally
        //     {
        //         // stream.Close();
        //     }


        // }
        // public async Task<List<ItemsToZip>> DownloadFiles(string productNumber)
        // {

        //     try
        //     {
        //         var streamImages = new List<ItemsToZip>();
        //         var images = await GetFileIdByProdId(productNumber);

        //         int i = 1;

        //         foreach (var img in images)
        //         {

        //             var fs = new GridFSBucket(_context._database);
        //             var fileInfo = await GetFileInfo(img.FileId, fs);
        //             var bytes = await fs.DownloadAsBytesAsync(img.FileId.ToObjectId());
        //             var itemsToZip = new ItemsToZip();
        //             itemsToZip.Name = string.Format("{0}_{1}{2}", productNumber, i, Path.GetExtension(fileInfo.Filename));
        //             // using (var stream = new MemoryStream(bytes))

        //             var stream = new MemoryStream(bytes);

        //             itemsToZip.Content = stream;
        //             i++;
        //             //  var bytes = await fs.DownloadAsBytesByNameAsync(@"test.txt");
        //             // await fs.DownloadToStreamByNameAsync(@"test.txt", stream);
        //             //  await fs.DownloadToStreamAsync("5ec43ce8ae36e1683829eeaa", stream);
        //             // await fs.DownloadToStreamAsync(img.FileID.ToObjectId(), stream);
        //             streamImages.Add(itemsToZip);
        //             // stream.Close();


        //         }
        //         return streamImages;


        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(String.Format("FileRepository-DownloadFile file:{0} {1}", System.Environment.NewLine, ex.ToString()));
        //         throw ex;
        //     }
        //     finally
        //     {
        //         // stream.Close();
        //     }


        // }
    }
}
