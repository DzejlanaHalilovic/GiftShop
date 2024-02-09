using AutoMapper;
using Micro.Async.User.Contracts.User.Request;
using Micro.Async.User.Contracts.User.Response;
using Micro.Async.User.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Micro.Async.User.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<Models.User> _context;
        private readonly IMapper _mapper;

        public UserService(UserManager<Models.User> context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<DeleteResponse> deleteUserAsync(int id)
        {
            var existingUser = await _context.FindByIdAsync(id.ToString());
            if (existingUser == null)
            {
                return new DeleteResponse { message = "User does not exist", success = false };
            }
            var result = await _context.DeleteAsync(existingUser);
            if (result.Succeeded)
                return new DeleteResponse { message = "User deleted successfully", success = true };
            return new DeleteResponse { message = "User could not be deleted", success = false };

        }

        public async Task<GetUserResponse> getUserByIdAsync(int id)
        {
            var user = await _context.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new GetUserResponse { message = "User does not exist", success = false, user = null };
            }
            return new GetUserResponse { user = user, message = "User is found", success = true };
        }

        public async Task<UpdateResponse> updateUserAsync(UpdateRequest request, int id)
        {
            var existingUser = await _context.FindByIdAsync(id.ToString());
            if (existingUser == null)
            {
                return new UpdateResponse { message = "User does not exist", success = false };
            }
            _mapper.Map(request, existingUser);
            var result = await _context.UpdateAsync(existingUser);
            if (!result.Succeeded)
            {
                return new UpdateResponse { message = "User could not be updated", success = false };
            }
            return new UpdateResponse { message = "User updated successfully", success = true };
        }

        public async Task<List<Models.User>> getUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
