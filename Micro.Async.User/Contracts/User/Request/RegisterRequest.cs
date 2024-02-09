﻿namespace Micro.Async.User.Contracts.User.Request
{
    public class RegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

    }
}
