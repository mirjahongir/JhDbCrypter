using JohaEfCrypter.Expressions;
using JohaEfCrypter.Intecepters;

using Microsoft.EntityFrameworkCore;

namespace ConsoleTestProject.Entities
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Person> Persons { get; set; } //=> (DbSet<Person>)Set<Person>().Encrypted();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("{{connectionString}}")
                .AddInterceptors(
                new CryptoInterceptor(),
                new DecryptioInterceptor()).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); ;
            base.OnConfiguring(optionsBuilder);
        }
    }
}
