using JohaEfCrypter.Intecepters;
using Microsoft.EntityFrameworkCore;

namespace ConsoleTestProject.Entities
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Person> Persons { get; set; } //=> (DbSet<Person>)Set<Person>().Encrypted();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connect = Environment.GetEnvironmentVariable("connect");
            if (string.IsNullOrEmpty(connect))
            {
                throw new Exception("Connectionstring error");
            }
            optionsBuilder.UseNpgsql(connect)
                .AddInterceptors(
                new CryptoInterceptor(),
                new DecryptioInterceptor()).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); ;
            base.OnConfiguring(optionsBuilder);
        }
    }
}
