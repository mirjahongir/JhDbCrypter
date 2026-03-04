using ConsoleTestProject.Entities.SqlEntity;
using JohaEfCrypter.Expressions;
using JohaEfCrypter.Intecepters;
using JohaEfCrypter.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace ConsoleTestProject
{
    public static class SqlExtension
    {
        public static IServiceCollection AddSqlService(this IServiceCollection service)
        {
            var connectString = Environment.GetEnvironmentVariable("connect");
            service.AddDbContext<DbContext, ApplicationDbContext>((sp, option) =>
            {
                option.UseNpgsql(connectString).AddInterceptors(
                    sp.GetRequiredService<CryptoInterceptor>(),
                    sp.GetRequiredService<DecryptioInterceptor>())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); ;
            });
            service.AddScoped<IInterceptorErrorHandler<Person>, PersonErrorHandler>();
            service.AddScoped<SqlService>();
            return service;
        }

    }
    // Bu Handler CheckSummasi hato bulgan Entitiylarni ushlab qolish uchun ishlatiladi
    public class PersonErrorHandler : IInterceptorErrorHandler<Person>
    {
        // Servicelarni DI qilamiz va shu error handlerni interceptor ichida ishlatamiz
        public PersonErrorHandler() { }

        public ValueTask ErrorHandler(Person obj)
        {
            // Check summasi hato chiqqan Entity
            Console.WriteLine($"Check summasi hato chiqqan Entity: {obj.Name}");
            return ValueTask.CompletedTask;
        }
    }

    public class SqlService
    {
        readonly DbContext _context;
        public SqlService(DbContext context)
        {
            _context = context;
        }
        public void AddPersonDb()
        {
            var person = _context.Set<Person>();
            var newPerson = new Person() { Name = "joha1", Password = "password", Pnfl = "123456" };
            person.Add(newPerson);
            _context.SaveChanges();
        }
        public void SearchByPersonName()
        {
            var personDb = _context.Set<Person>();
            List<string> names = ["joha1"];
            var person = personDb.Encrypted().Where(m => m.Name!="joha1").FirstOrDefault();
            Console.WriteLine(person?.Name);

        }
    }
}
