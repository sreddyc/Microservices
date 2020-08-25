using System.Text.Json;
using API.Helpers;
using Microsoft.AspNetCore.Http;
// using Newtonsoft.Json;
// using Newtonsoft.Json.Serialization;

namespace API.Extensions
{
    public static class Pagination
    {
        public static void AddPagination(this HttpResponse response,
           int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);

             var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

            var json = JsonSerializer.Serialize(paginationHeader, options);
                
           // var camelCaseFormatter = new JsonSerializerSettings();
            //camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination",  json);
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
        
    }
}