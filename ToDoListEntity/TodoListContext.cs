using MongoDB.Driver;
using TodoListApp.Entities.Models;

namespace TodoListApp.Entities;

public class TodoListContext
{
    private readonly IMongoDatabase _database;

    public TodoListContext(TodoListDatabaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.DatabaseName);
    }
    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    public IMongoCollection<TodoList> TodoLists => _database.GetCollection<TodoList>("TaskLists");
   
}