### Joha Asp Crypter 
Asp .net dasturiga integratsiya qilish uchun ishlatiladi

```
services.RegisterJhCrypter(option =>
{
    option.Key = "test_test_test";
    option.EncryptType = JohaEfCrypter.Enums.EncryptType.AesCbc;  
});
```
