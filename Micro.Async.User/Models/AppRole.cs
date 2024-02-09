using Microsoft.AspNetCore.Identity;

namespace Micro.Async.User.Models
{
    public class AppRole : IdentityRole<int>
    {
        public AppRole(string roleName) : base(roleName)
        {
        }

        public AppRole()
        {
        }
    }
}
