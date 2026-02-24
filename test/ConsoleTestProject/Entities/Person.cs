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
        [Encrypted(IsEncrypt = true)]
        public string? Name { get; set; }
        [Encrypted(CheckSum = true)]
        public string? chech_sum { get; set; }


    }
}
