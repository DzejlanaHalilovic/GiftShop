using AutoMapper;
using Micro.Async.User.Contracts.User.Request;
using Micro.Async.User.Interfaces;
using Micro.Async.User.Persistance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Micro.Async.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IMessageBroker _broker;
        public IdentityController(IIdentityService identityService, IMapper mapper, IMessageBroker messageBroker)
        {
            _identityService = identityService;
            _mapper = mapper;
            _broker = messageBroker;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _identityService.Login(request);
            if (response.Error != null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Contracts.User.Request.RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userR = _mapper.Map<Models.User>(request);

            var response = await _identityService.Register(userR, request.Password);
            if (response.Error != null)
            {
                return BadRequest(response);
            }
            _broker.Consume();
            return Ok(response);
        }






    }
}
