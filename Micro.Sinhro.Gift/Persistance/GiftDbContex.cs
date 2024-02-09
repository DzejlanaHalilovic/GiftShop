


using Microsoft.EntityFrameworkCore;

namespace Micro.Sinhro.Gift.Persistance
{
    public class GiftDbContex : DbContext
    {
        public GiftDbContex(DbContextOptions<GiftDbContex> options) : base(options)
        {
        }
        public DbSet<Models.Gift> Gifts { get; set; }
    }
}
