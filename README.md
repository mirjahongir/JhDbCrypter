## Joha Ef Core Encryptor 
 Created for Crypte and Encrypt property 
 Dastur hali alfa versiyada yaqinda tuliq versiya chiqadi

 ``` 
 public class ApplicationContext : DbContext
    {
        public DbSet<Person> Persons { get; set; } //=> (DbSet<Person>)Set<Person>().Encrypted();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("{{connectionString}}")
                .AddInterceptors(
                // Save qilishda Ma`lumotlarni shifirlaydi
                new CryptoInterceptor(),
                // Select qilganda Decrypt qiladi
                new DecryptioInterceptor()).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); ;
            base.OnConfiguring(optionsBuilder);
        }
    }

 ```

 Tablitsaga 

 ```
  [Table("person_test")]
  // qaysi tablitsada Encrypt qilinadigan bulsa ustida bulishi kerak
    [EncryptTable]
    public class Person
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Column("name")]
        // Qaysi field Shifirlanishi kerak bulsa Encrypted Attribute bulishi kerak
        [Encrypted(IsEncrypt =true)]
        ///
        public string? Name { get; set; }
        [Column("hash_field")]
        // IsHash bu Polyani  bazaga hashlab quyadi
        boshlang`ich qismiga hash qushib quyadi
        [Encrypted(IsHash =true)]
        public string? HashField { get; set; }
    }
 ```