using JohaEfCrypter.Extensions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;

namespace JhMongoCrypter.Intecepters.Serializer
{
    internal class EncryptedStringSerializer : SerializerBase<string>
    {
        const string prefix = "Crypt:";
        public override void Serialize(BsonSerializationContext context,
            BsonSerializationArgs args, string value)
        {

            if (value == null)
            {
                context.Writer.WriteNull();
                return;
            }

            context.Writer.WriteString(prefix + CryptoExtension.EncryptStr(value));
        }

        public override string Deserialize(BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            var value = context.Reader.ReadString();
            if (value.StartsWith(prefix))
            {
                return CryptoExtension.DecryptBase64(value.Substring(prefix.Length));
            }
            return value;
          //  return base.Deserialize(context, args);
            //return CryptoExtension.DecryptBase64(context.Reader.ReadString());
        }
    }
}
