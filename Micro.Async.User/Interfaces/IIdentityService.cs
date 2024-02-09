using Micro.Async.User.Contracts.User.Request;
using Micro.Async.User.Contracts.User.Response;

namespace Micro.Async.User.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthenticationResponse> Register(Models.User request, String password);

        Task<AuthenticationResponse> Login(LoginRequest request);
    }
}
