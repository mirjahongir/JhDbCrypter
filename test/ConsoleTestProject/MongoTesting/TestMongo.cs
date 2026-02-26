using ConsoleTestProject.Entities;
using JhMongoCrypter.Intecepters;

using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace ConsoleTestProject.MongoTesting
{
    public class TestMongo
    {
        public static void MongoTest()
        {

            var pack = new ConventionPack { new EncryptedMemberConvention() };

            ConventionRegistry.Register(
                "EncryptedMembers",
                pack,
                _ => true
            );
            //"mongodb://admin:joha123456@192.168.20.242:27018"
            MongoClient client = new MongoClient("mongodb://admin:joha123456@192.168.20.242:27018");
            var database = client.GetDatabase("test_encrypt");
            var person = database.GetCollection<MongoPerson>("person");

            for (int i = 0; i < 10; i++)
            {
                person.InsertOne(new MongoPerson() { Name = $"joha{i}", Password="test Password" });
            }

            Console.WriteLine("Insert Finish");
            var firstJoha = person.Find(m => m.Name == "joha1").FirstOrDefault();
            Console.WriteLine($"First Joha: {firstJoha.Name} Id: {firstJoha.Id} , Password: {firstJoha.Password}");



        }
    }
}
