using ArchiLibrary.Data;
using ArchiLibrary.Models;
using ArchiLog.Models;
using Microsoft.EntityFrameworkCore;

namespace ArchiLog.Data
{
    public class ArchiLogDbContext : BaseDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=Archi;User=sa;Password=Docker@123;");
        }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Car> Cars { get; set; }
    }
}
