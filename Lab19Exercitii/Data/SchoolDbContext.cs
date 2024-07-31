using Lab19Exercitii.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab19Exercitii.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
