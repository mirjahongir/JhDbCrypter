using ConsoleTestProject;
using JohaAspCrypter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#region Sql Service Misol
var services = new ServiceCollection();
// 2. Service'larni ro‘yxatdan o‘tkazamiz
services.RegisterJhCrypter(option =>
{
    option.Key = "test_test_test";
    option.EncryptType = JohaEfCrypter.Enums.EncryptType.AesCbc;
});

Console.WriteLine("Start Program");

services.AddSqlService();
var build = services.BuildServiceProvider();
var context = build.GetRequiredService<DbContext>();
context.Database.EnsureDeleted();
context.Database.EnsureCreated();
build.UpdateDb();
var service = build.GetRequiredService<SqlService>();

service.AddPersonDb();

Console.ReadLine();
service.SearchByPersonName();
Console.ReadLine();
#endregion


#region MongoDb uchun misol
var mongoService = new ServiceCollection();
services.RegisterJhCrypter(option =>
{
    option.Key = "mongo_test_key";
    option.EncryptType = JohaEfCrypter.Enums.EncryptType.AesCbc;
});
mongoService.AddMongoService();
var builder = mongoService.BuildServiceProvider();
var mService = builder.GetRequiredService<MongoService>();

mService.InserMongoPerson();
var personName = mService.GetbyName();
Console.WriteLine(personName.Name);
var passPerson = mService.GetByPassword();
Console.WriteLine(passPerson.Name);
Console.ReadLine();

#endregion