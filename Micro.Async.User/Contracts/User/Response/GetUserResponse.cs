namespace Micro.Async.User.Contracts.User.Response
{
    public class GetUserResponse
    {
        public string message { get; set; }
        public bool success { get; set; }
        public Models.User user { get; set; }
    }
}
