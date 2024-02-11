using Micro.Sinhro.REST.APIGateway.Contracts.User.Request;
using Micro.Sinhro.REST.APIGateway.Contracts.User.Response;
using Micro.Sinhro.REST.APIGateway.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace Micro.Sinhro.REST.APIGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly Urls url;

        public UsersController(IOptions<Urls> url, HttpClient httpClient)
        {
            this.url = url.Value;
            this.httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = httpClient.GetAsync(url.Users + "/User/all").Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var users = JsonConvert.DeserializeObject<List<User>>(content);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var response = httpClient.GetAsync(url.Users + "/User/" + id).Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<User>(content);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateRequest request, int id)
        {
            var json = JsonConvert.SerializeObject(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = httpClient.PutAsync(url.Users + "/User/" + id, data).Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<DeleteUpdateResponse>(content);
            if (result == null)
            {
                return BadRequest(new { message = "error while deserialization", succes = false });
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = httpClient.DeleteAsync(url.Users + "/User/" + id).Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<DeleteUpdateResponse>(content);
            if (result == null)
            {
                return BadRequest(new { message = "error while deserialization", succes = false });
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync(url.Users + "/Identity/login", data).Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<AuthenticationResponse>(content);
            if (result == null)
            {
                return BadRequest(new { message = "error while deserialization", succes = false });
            }
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Contracts.User.Request.RegisterRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync(url.Users + "/Identity/register", data).Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<AuthenticationResponse>(content);
            if (result == null)
            {
                return BadRequest(new { message = "error while deserialization", succes = false });
            }
            return Ok(result);
        }
    }
}
