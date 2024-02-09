namespace Micro.Async.User.Contracts.User.Response
{
    public class AuthenticationResponse
    {
        public string? Token { get; set; }

        public Models.User? User { get; set; }

        public string? Role { get; set; }

        public string? Error { get; set; }

        public int StatusCode { get; set; }

    }
}
