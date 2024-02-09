using Micro.Async.User.Contracts.User.Request;
using Micro.Async.User.Interfaces;
using Micro.Async.User.Persistance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Micro.Async.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMessageBroker _messageBroker;

        public UserController(IUserService userService, IMessageBroker messageBroker)
        {
            _userService = userService;
            _messageBroker = messageBroker;
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.getUsersAsync();
            //_messageBroker.Publish(users);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.getUserByIdAsync(id);
            if (!result.success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateRequest request, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.updateUserAsync(request, id);
            if (!result.success)
            {
                return BadRequest(result);
            }
            // _messageBroker.Publish(result.message);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.deleteUserAsync(id);
            if (!result.success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
