### JohaDev MongoDb uchun Encrypt va Decrypt qilish uchun kutubxona

```
dasturni boshida shu qushiladi

var pack = new ConventionPack { new EncryptedMemberConvention() };

            ConventionRegistry.Register(
                "EncryptedMembers",
                pack,
                _ => true
            );

```