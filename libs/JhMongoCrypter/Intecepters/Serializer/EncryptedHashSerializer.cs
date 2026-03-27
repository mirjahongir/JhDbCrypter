using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using JhCrypter;

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
            //if (value.StartsWith(HeshPrefix, StringComparison.OrdinalIgnoreCase))
            //    return;
            context.Writer.WriteString(value.HashString());
        }
        public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var value = context.Reader.ReadString();
            if (string.IsNullOrEmpty(value)) return value;
            return value;
        }
    }
}
