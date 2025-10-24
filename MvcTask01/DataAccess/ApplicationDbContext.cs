using ContactDoctor.Models;
using Microsoft.EntityFrameworkCore;
using MvcTask01.Models;

namespace MvcTask01.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=T0P\\SQLEXPRESS3;Initial Catalog=ERM_521;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Doctor>().HasKey(d => d.Id);

            var sample = new SampleDataDoctor();

            modelBuilder.Entity<Doctor>().HasData(sample.doctors);

        }
    }
}
