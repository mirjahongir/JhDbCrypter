using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JhCrypter.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace ConsoleTestProject.Entities
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
    [Table("person_test")]
    [EncryptTable]
    public class Person
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Column("name")]
        [Encrypted(IsEncrypt = true)]
        public string? Name { get; set; }
        [Encrypted(CheckSum = true)]
        public string? chech_sum { get; set; }


    }
}
