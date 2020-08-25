using System.Net.Http;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Dtos;

namespace Infrastructure.Helpers
{
    public class InvokeBasketService
    {
        static HttpClient client = new HttpClient();
        public static async Task<CustomerBasket> GetCustomerBasketAsync(string path)
        {
            CustomerBasket custBasket = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                custBasket = await response.Content.ReadAsAsync<CustomerBasket>();
            }
            return custBasket;
        }

        public static async Task<CustomerBasket> UpdateCustomerBasketAsync(string path, CustomerBasket basket)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                path, basket);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            basket = await response.Content.ReadAsAsync<CustomerBasket>();
            return basket;
        }
    }
}