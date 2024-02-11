using Micro.Sinhro.REST.APIGateway.Contracts.Gift.Request;
using Micro.Sinhro.REST.APIGateway.Contracts.Gift.Response;
using Micro.Sinhro.REST.APIGateway.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Micro.Sinhro.REST.APIGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly Urls url;

        public GiftController(HttpClient httpClient, IOptions<Urls> url)
        {
            this.httpClient = httpClient;
            this.url = url.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGift()
        {
            var response = httpClient.GetAsync(url.Gifts + "/Gift").Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var users = JsonConvert.DeserializeObject<List<Gift>>(content);
            return Ok(users);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetGiftById(int id)
        {
            var response = httpClient.GetAsync(url.Gifts + "/Gift/" + id).Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var car = JsonConvert.DeserializeObject<Gift>(content);
            return Ok(car);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGiftById(int id)
        {
            var response = httpClient.DeleteAsync(url.Gifts + "/Gift/" + id).Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var car = JsonConvert.DeserializeObject<DeleteResponse>(content);
            return Ok(car);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGift([FromForm] CreateRequest gift)
        {
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(new StringContent(gift.Name), "Name");
                formData.Add(new StringContent(gift.Description), "Description");
                formData.Add(new StringContent(gift.Price.ToString()), "Price");  
                var response = httpClient.PostAsync(url.Gifts + "/Gift", formData).Result;
                 response.EnsureSuccessStatusCode();

                 var content = response.Content.ReadAsStringAsync().Result;
                 var result = JsonConvert.DeserializeObject<CreateResponse>(content);
                 return Ok(result);
                



            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGift([FromForm] UpdateRequest request, int id)
        {
            using (var formData = new MultipartFormDataContent())
            {
                // Dodajte ostale tekstualne podatke

                formData.Add(new StringContent(request.Description), "Description");
                formData.Add(new StringContent(request.Price.ToString()), "Price");

                var response = httpClient.PutAsync(url.Gifts + "/Gift/" + id, formData).Result;
                response.EnsureSuccessStatusCode();

                var content = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<UpdateResponse>(content);
                return Ok(result);



            }


        }


    }
}
