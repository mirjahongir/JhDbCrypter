using JohaEfCrypter.Intecepters;
using Microsoft.EntityFrameworkCore;

namespace ConsoleTestProject.Entities.SqlEntity
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option) { }
        public DbSet<Person> Persons { get; set; } 
    }
}
