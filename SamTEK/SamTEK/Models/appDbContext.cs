using Microsoft.EntityFrameworkCore;

namespace SamTEK.Models
{
    public class appDbContext : DbContext
    {
        public appDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<servisTablosu> servisTablosu { get; set; }
        public DbSet<cihazTablosu> cihazTablosu { get; set; }
        public DbSet<kullaniciTablosu> kullaniciTablosu { get; set; }


    }
}
