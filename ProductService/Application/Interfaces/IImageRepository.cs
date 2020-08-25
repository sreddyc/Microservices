using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

namespace Application.Interfaces
{
    public interface IImageRepository
    {
         Task<ObjectId> Upload(Image image);
       //  Task<IEnumerable<Image>> Get(List<ObjectId> ids);
         List<byte[]> Get(List<ObjectId> ids);
         Task<Image> Get(ObjectId id);

         Task<string> GetImgBase64(ObjectId id);
        //  Task<string> UploadFile(IFormFile files, string productNumber);
        // // Task<List<ItemsToZip>> DownloadFiles(string productNumber);
        // Task<List<byte[]>> DownloadFilesBytes(string productNumber);

        // Task<string> UploadFile(byte[] fileBytes, string productId);

        //  Task<long> CountAsync();

        //  Task<bool> Delete(string productNumber);
    }
}