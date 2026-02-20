using ConsoleTestProject.Entities;

using JohaEfCrypter.Extensions;

Console.WriteLine("Hello, World!");


//using var db = new ApplicationContext();

//db.Database.EnsureDeleted();
//db.Database.EnsureCreated();
var str = "joha".EncryptStr();
var str1 = "joha".EncryptStr();
var str2= "joha".EncryptStr();
Console.WriteLine(str);
Console.WriteLine(str1);
Console.WriteLine(str2);
//var person1 = new Person() { Name = "joha", };
//db.Persons.Add(person1);
// db.SaveChanges();
//Console.WriteLine("Encrypt Value:  " + person1.Name);
var str3 = "joha".EncryptStr();
// var findJoha = db.Persons.Where(m => m.Name == str).FirstOrDefault();


Console.WriteLine("Decripted Value");
//Console.WriteLine(findJoha?.Name);
Console.WriteLine("Item 1 Item2");
Console.ReadLine();


