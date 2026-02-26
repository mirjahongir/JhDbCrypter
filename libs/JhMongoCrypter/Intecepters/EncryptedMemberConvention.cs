using JhCrypter.Attributes;
using JhMongoCrypter.Intecepters.Serializer;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using System.Reflection;

namespace JhMongoCrypter.Intecepters
{
    public class EncryptedMemberConvention : IMemberMapConvention
    {
        public string Name => "EncryptedMemberConvention";

        public void Apply(BsonMemberMap memberMap)
        {
            if (memberMap.MemberType != typeof(string))
                return;

            var attr = memberMap.MemberInfo.GetCustomAttribute<EncryptedAttribute>();
            if (attr == null) return;
            if (attr.IsHash)
            {
                memberMap.SetSerializer(new EncryptedHashSerializer());
            }
            if (attr.IsEncrypt)
            {
                memberMap.SetSerializer(new EncryptedStringSerializer());
            }

        }
    }
}
