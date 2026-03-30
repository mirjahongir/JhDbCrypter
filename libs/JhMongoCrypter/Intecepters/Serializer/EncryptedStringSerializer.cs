using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using JhCrypter;

namespace JhMongoCrypter.Intecepters.Serializer
{
    internal class EncryptedStringSerializer : SerializerBase<string>
    {
        public override void Serialize(BsonSerializationContext context,
             BsonSerializationArgs args, string value)
        {
            if (value == null)
            {
                context.Writer.WriteNull();
                return;
            }
            context.Writer.WriteString(CryptoExtension.EncryptStr(value));
        }

        public override string Deserialize(BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            var value = context.Reader.ReadString();
            return CryptoExtension.DecryptBase64(value);
        }
    }
}
