using ConsoleTestProject.Entities;
using ConsoleTestProject.MongoTesting;
using JhCrypter.Config;
using JohaAspCrypter;
using JohaEfCrypter.Expressions;
using JohaEfCrypter.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
// 2. Service'larni ro‘yxatdan o‘tkazamiz
services.AddDbContext<DbContext, ApplicationContext>();
services.RegisterJhCrypter(option =>
{
    option.Key = "test_test_test";
    option.EncryptType = JohaEfCrypter.Enums.EncryptType.AesCbc;  
});

Console.WriteLine("Hello, World!");
var connect = Environment.GetEnvironmentVariable("connect");

TestMongo.MongoTest();
//CryptConfig.Option = new CryptOption() { Key = "test_test_test" };
//var dec1 = "gJAy8J8aoDW+GJy+sdPbYLoMEsq4ZMdnjHMLM6TtQRP0k1bZhRD9scUzuTJnvTKt".DecryptBase64();
//var enc = "joha".EncryptStr();
//var dec = enc.DecryptBase64();
return;

// 1. IServiceCollection yaratamiz
var build = services.BuildServiceProvider();
build.UpdateDb();

var context = build.GetService<DbContext>();
var _person = context.Set<Person>();


var person = _person.Encrypted().Where(m => m.Name== "joha1").FirstOrDefault();
Console.WriteLine(person.Name);
_person.Add(new Person() { Name = "joha1" });
context.SaveChanges();

Console.WriteLine(person.Name);


return;
CryptConfig.Option = new CryptOption() { EncryptType = JohaEfCrypter.Enums.EncryptType.AesCbc, Key = "Helloworlkd" };
using var db = new ApplicationContext();

//db.Database.EnsureDeleted();
//db.Database.EnsureCreated();
//Console.WriteLine(str);
//Console.WriteLine(str1);
//Console.WriteLine(str2);
var person1 = new Person() { Name = "joha", };
db.Persons.Add(person1);
db.SaveChanges();
//Console.WriteLine("Encrypt Value:  " + person1.Name);
var str3 = "joha".EncryptStr();
// var findJoha = db.Persons.Where(m => m.Name == str).FirstOrDefault();


Console.WriteLine("Decripted Value");
//Console.WriteLine(findJoha?.Name);
Console.WriteLine("Item 1 Item2");
Console.ReadLine();


