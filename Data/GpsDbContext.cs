using Microsoft.EntityFrameworkCore;
using GPS_Listener.Models;

namespace GPS_Listener.Data
{
    public class GpsDbContext : DbContext
    {
        public DbSet<LocationEntity> Locations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(
                "Server=localhost;Database=GPS_DB;Trusted_Connection=True;TrustServerCertificate=True;"
            );
        }
    }
}