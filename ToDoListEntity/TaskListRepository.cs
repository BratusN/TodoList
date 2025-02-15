using MongoDB.Driver;
using TodoListApp.Entities.Models;

namespace TodoListApp.Entities;

public class TodoListRepository : ITodoListRepository
{
    private readonly IMongoCollection<TodoList> _TodoLists;
    private readonly IMongoCollection<User> _Users;

    public TodoListRepository(TodoListContext context)
    {
        _TodoLists = context.TodoLists;
        _Users = context.Users;
    }

    public async Task CreateAsync(TodoList TodoList)
    {
        await _TodoLists.InsertOneAsync(TodoList);
    }

    public async Task UpdateAsync(TodoList TodoList)
    {
        await _TodoLists.ReplaceOneAsync(t => t.Id == TodoList.Id, TodoList);
    }

    public async Task DeleteAsync(string id)
    {
        await _TodoLists.DeleteOneAsync(t => t.Id == id);
    }

    public async Task<TodoList?> GetByIdAsync(string id)
    {
        return await _TodoLists.Find(t => t.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<TodoList>> GetUserTodoListsAsync(string userId, int page, int pageSize)
    {
        var filter = Builders<TodoList>.Filter.Or(
            Builders<TodoList>.Filter.Eq(t => t.OwnerId, userId),
            Builders<TodoList>.Filter.AnyEq(t => t.SharedWith, userId)
        );

        return await _TodoLists.Find(filter)
            .SortByDescending(t => t.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(string id)
    {
        return await _Users.Find(t => t.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User> CreateUser(User user)
    {
        await _Users.InsertOneAsync(user);
        return user;
    }
}