using Microsoft.EntityFrameworkCore;
using MvcTask01.Models;

namespace MvcTask01.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Doctor>().HasKey(d => d.Id);

            var sample = new SampleDataDoctor();
            modelBuilder.Entity<Doctor>().HasData(sample.doctors);
        }
    }
}