using ConsoleTestProject.Entities.MongoEntity;
using JhMongoCrypter.Intecepters;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace ConsoleTestProject
{
    public static class MongoExtension
    {
        public static IServiceCollection AddMongoService(this IServiceCollection services)
        {
            var pack = new ConventionPack { new EncryptedMemberConvention() };

            ConventionRegistry.Register(
                "EncryptedMembers",
                pack,
                _ => true
            );
            var mongoConnect = Environment.GetEnvironmentVariable("mongoConnect");
            MongoClient client = new MongoClient(mongoConnect);
            var database = client.GetDatabase("test_encrypt");
            services.AddSingleton(database);
            services.AddScoped<MongoService>();
            return services;
        }
    }
    public class MongoService
    {
        #region Default Constructor
        readonly IMongoDatabase _db;
        readonly IMongoCollection<MongoPerson> _person;
        public MongoService(IMongoDatabase db)
        {
            _db = db;
            _person = db.GetCollection<MongoPerson>("crypt_person");
        }
        #endregion
        public void InserMongoPerson()
        {
            MongoPerson person = new() { Name = "joha", Password = "MyPassword" };
            _person.InsertOne(person);

        }
        public MongoPerson GetbyName()
        {
            var person = _person.Find(m => m.Name == "joha").FirstOrDefault();
            return person;
        }
        public MongoPerson GetByPassword()
        {
            var person = _person.Find(m => m.Password == "MyPassword").FirstOrDefault();
            return person;
        }

    }
}
