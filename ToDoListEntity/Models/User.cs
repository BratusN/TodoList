using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoListApp.Entities.Models;
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedDate { get; set; }

    public required string Name { get; set; }

}

