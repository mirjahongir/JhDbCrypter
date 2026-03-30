using ConsoleTestProject;
using JohaAspCrypter;
using Microsoft.Extensions.DependencyInjection;
using JhCrypter;
using JhCrypter.Config;
var services = new ServiceCollection();
// 2. Service'larni ro‘yxatdan o‘tkazamiz
services.RegisterJhCrypter(option =>
{
    option.Key = "test_test_test";
    option.EncryptType = JohaEfCrypter.Enums.EncryptType.AesCbc;
});
#region List
var list = new List<string>
{
    "hello world my joha",
    "lwkemdlwemdlw eldkwemdlkwe",
    "wedmwledwp weldmweodmkw",
    "w;eodkjw;elmdw;leodwle lwem",
};
#endregion

foreach (var crypt in list)
{
    var first = crypt.EncryptStr();
    var data = Convert.FromBase64String(first);
    var decrypt = first.DecryptBase64();
    var second = first.EncryptStr();
    if (first != second)
    {
        Console.WriteLine("Not scdssdcsd sldcinsdl");
    }
}

//foreach (var i in list)
//{
//    var firstHash = i.HashString(false);
//    var secondHash = firstHash.HashString();
//    Console.WriteLine(firstHash);
//    if (firstHash != secondHash)
//    {
//        Console.WriteLine("Not Exist");
//    }
//}
//Console.WriteLine("FinishJob");
//return;



#region Sql Service Misol

//var str = "joheHwllod cs lsdmcfslken lendfw";
//var hash = str.HashString();
//var hash2 = hash.HashString();

//if (hash == hash2)
//{

//}
//Console.WriteLine(hash);
//var first = str.EncryptStr();
//var second = first.EncryptStr();
//if (first == second)
//{
//    Console.WriteLine("Hwllo rwepmfwe");
//}



return;
//var services = new ServiceCollection();
//// 2. Service'larni ro‘yxatdan o‘tkazamiz
//services.RegisterJhCrypter(option =>
//{
//    option.Key = "test_test_test";
//    option.EncryptType = JohaEfCrypter.Enums.EncryptType.AesCbc;
//});

//Console.WriteLine("Start Program");

//services.AddSqlService();
//var build = services.BuildServiceProvider();
////var context = build.GetRequiredService<DbContext>();
////context.Database.EnsureDeleted();
////context.Database.EnsureCreated();
//build.UpdateDb();
//var service = build.GetRequiredService<SqlService>();
////service.AddPersonDb();

////Console.ReadLine();
//service.SearchByPersonName();
//Console.ReadLine();
//#endregion


//#region MongoDb uchun misol
//var mongoService = new ServiceCollection();
//services.RegisterJhCrypter(option =>
//{
//    option.Key = "mongo_test_key";
//    option.EncryptType = JohaEfCrypter.Enums.EncryptType.AesCbc;
//});
//mongoService.AddMongoService();
//var builder = mongoService.BuildServiceProvider();
//var mService = builder.GetRequiredService<MongoService>();

//mService.InserMongoPerson();
//var personName = mService.GetbyName();
//Console.WriteLine(personName.Name);
//var passPerson = mService.GetByPassword();
//Console.WriteLine(passPerson.Name);
//Console.ReadLine();

#endregion