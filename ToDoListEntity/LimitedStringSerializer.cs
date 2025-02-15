using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace TodoListApp.Entities
{

    public class LimitedStringSerializer : SerializerBase<string>
    {
        private readonly int _maxLength;

        public LimitedStringSerializer(int maxLength)
        {
            _maxLength = maxLength;
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string value)
        {
            if(value.Length > _maxLength)
                throw new BsonSerializationException($"String length exceeds {_maxLength} characters.");

            context.Writer.WriteString(value);
        }

        public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return context.Reader.ReadString();
        }
    }
}
