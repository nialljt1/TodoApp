using Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Data.Model;

namespace Data.Model
{
    public partial class AppContext : BaseDbContext
    {
        public AppContext()
            : base("name=ApplicationDatabase")
        {
        }

        public AppContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; }
    }
}