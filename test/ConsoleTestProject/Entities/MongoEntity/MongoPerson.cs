using JhCrypter.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace ConsoleTestProject.Entities.MongoEntity
{
    public class MongoPerson
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [Encrypted(IsEncrypt = true)]
        public string? Name { get; set; }
        [Encrypted(IsHash = true)]
        public string? Password { get; set; }


    }
}
