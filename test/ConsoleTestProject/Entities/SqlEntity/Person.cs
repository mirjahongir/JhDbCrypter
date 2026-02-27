using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JhCrypter.Attributes;


namespace ConsoleTestProject.Entities.SqlEntity
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
        [Column("password")]
        [Encrypted(IsHash = true)]
        public string? Password { get; set; }
        [Column("pnfl")]
        [Encrypted(IsEncrypt = true)]
        public string? Pnfl { get; set; }
        [Column("password_hash")]
        [Encrypted(HashField = nameof(Pnfl))]
        public string? PnflHash { get; set; }
        [Encrypted(CheckSum = true)]
        public string? chech_sum { get; set; }


    }
}
