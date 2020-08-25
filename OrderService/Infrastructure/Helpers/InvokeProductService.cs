using System.Net.Http;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Dtos;

namespace Infrastructure.Helpers
{
    public class InvokeProductService
    {
        static HttpClient client = new HttpClient();
        public static async Task<ProductDto> GetProductAsync(string path)
        {
            ProductDto product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<ProductDto>();
            }
            return product;
        }
    }
}