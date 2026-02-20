using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using JohaEfCrypter.Attributes;

namespace ConsoleTestProject.Entities
{
    [Table("person_test")]
    [EncryptTable]
    public class Person
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Column("name")]
        [Encrypted(IsEncrypt =true)]
        public string? Name { get; set; }
        [Column("hash_field")]
        [Encrypted(IsHash =true)]
        public string? HashField { get; set; }


    }
}
