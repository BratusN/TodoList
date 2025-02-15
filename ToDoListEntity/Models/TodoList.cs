using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Entities.Models;

public class TodoList
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    [StringLength(255, ErrorMessage = "Name must be under 255 characters.")]
    public required string Name { get; set; }

    public required string OwnerId { get; set; }

    public List<TodoItem> TodoItems { get; set; } = [];

    public List<string> SharedWith { get; set; } = [];
}

