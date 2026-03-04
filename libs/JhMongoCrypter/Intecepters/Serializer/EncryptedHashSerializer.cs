using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace JhMongoCrypter.Intecepters.Serializer
{
    internal class EncryptedHashSerializer : SerializerBase<string>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                context.Writer.WriteString("");
                return;
            }
            if (value.StartsWith(hash, StringComparison.OrdinalIgnoreCase))
                return;
            context.Writer.WriteString(hash + value.HashString());
        }
        public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var value = context.Reader.ReadString();
            if (string.IsNullOrEmpty(value)) return value;
            return value;
        }
    }
}
