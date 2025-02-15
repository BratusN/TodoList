using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoListApp.Entities.Models;

public class TodoList
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
  //  [BsonSerializer()]//TODO
    public required string Name { get; set; }

    public required string OwnerId { get; set; }

    public List<TodoItem> TodoItems { get; set; } = [];

    public List<string> SharedWith { get; set; } = [];
}

