using Micro.Sinhro.REST.ApiGateway.Contracts.UserGift.Request;
using Micro.Sinhro.REST.ApiGateway.Contracts.UserGift.Response;
using Micro.Sinhro.REST.ApiGateway.Models;
using Micro.Sinhro.REST.APIGateway;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace Micro.Sinhro.REST.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGiftController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly Urls url;

        public UserGiftController(HttpClient httpClient, IOptions<Urls> url)
        {
            this.httpClient = httpClient;
            this.url = url.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = httpClient.GetAsync(url.UserGift + "/usercars").Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var users = JsonConvert.DeserializeObject<List<UserGiftResponse>>(content);
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync(url.UserGift + "/create-usergift", data).Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<CreateResponse>(content);
            if (result == null)
            {
                return BadRequest(new { message = "error while deserialization", succes = false });
            }

            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = httpClient.GetAsync(url.UserGift + "/usergift/" + id).Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<UserGift>(content);
            return Ok(user);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(string id)
        {
            var response = httpClient.DeleteAsync(url.UserGift + "/delete-usergift/" + id).Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<UserGift>(content);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] UpdateRequest request, string id)
        {
            var json = JsonConvert.SerializeObject(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = httpClient.PutAsync(url.UserGift + "/put-usergift/" + id, data).Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<UserGift>(content);
            if (result == null)
            {
                return BadRequest(new { message = "error while deserialization", succes = false });
            }
            return Ok(result);
        }
    }
}
