using Microsoft.EntityFrameworkCore;

namespace EcsDigitalApi.Entities
{
    public class CarsContext : DbContext
    {
        public CarsContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Car> Cars { get; set; }
    }
}