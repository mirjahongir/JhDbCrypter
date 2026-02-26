## Joha Ef Core Encryptor 
 Created for Crypte and Encrypt property 
 Dastur hali alfa versiyada yaqinda tuliq versiya chiqadi

 ``` 
 services.AddDbContext<DbContext, ApplicationDbContext>((sp, option) =>
            {
                option.UseNpgsql(GeneralEnv.PostgresConnection)

                .AddInterceptors(
                    // Db ga saqlashdan  Shifirlash Interceptor
                    sp.GetRequiredService<CryptoInterceptor>(),
                    // Bazadan olganda Deshifirlash Interceptor
                    sp.GetRequiredService<DecryptioInterceptor>(),

                    sp.GetRequiredService<ValidationSaveChangesInterceptor>())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            return services;

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

 ## Deshifirlash jarayonida Consistend hatolik bulsa shu modelni null qilib qaytaradi va Error Handlerga ma`lumot junatadi

 Error Handlerni Registratsiya qilish

 ```
                                          //Model Quyiladi 
 service.AddScoped<IInterceptorErrorHandler<Person>, Your_Error_Handler>()
 ```