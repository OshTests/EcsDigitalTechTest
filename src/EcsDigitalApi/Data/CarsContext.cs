using EcsDigitalApi.DbModels;
using Microsoft.EntityFrameworkCore;

namespace EcsDigitalApi.Data
{
    public class CarsContext : DbContext
    {
        public CarsContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>().HasKey(p => p.Id);
            modelBuilder.Entity<Car>().Property(p => p.ModelId).IsRequired();
            modelBuilder.Entity<Car>().Property(p => p.Colour).IsRequired();
            modelBuilder.Entity<Car>().Property(p => p.Year).IsRequired();

            modelBuilder.Entity<Maker>().HasKey(p=>p.Id);
            modelBuilder.Entity<Maker>().Property(p => p.Name).IsRequired();
            
            modelBuilder.Entity<Model>().HasKey(p=>p.Id);
            modelBuilder.Entity<Model>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<Model>().Property(p => p.MakerId).IsRequired();

            modelBuilder.Entity<Car>().HasOne(p => p.Model);
            modelBuilder.Entity<Model>().HasOne(p => p.Maker).WithMany(p => p.Models);
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Maker> Makers { get; set; }
        public DbSet<Model> Models { get; set; }
    }
}