using Micro.Async.User.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Micro.Async.User.Persistance
{
    public class UserDbContext : IdentityDbContext<Models.User, AppRole, int>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public UserDbContext()
        {
        }
    }
}
