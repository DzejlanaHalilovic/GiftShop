using Micro.Async.User.Contracts.User.Request;
using Micro.Async.User.Contracts.User.Response;

namespace Micro.Async.User.Interfaces
{
    public interface IUserService
    {
        Task<GetUserResponse> getUserByIdAsync(int id);
        Task<List<Models.User>> getUsersAsync();

        Task<UpdateResponse> updateUserAsync(UpdateRequest request, int id);
        Task<DeleteResponse> deleteUserAsync(int id);
    }
}
