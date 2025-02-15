using TodoListApp.Entities.Models;

namespace TodoListApp.Entities;
/// <summary>
/// should be generic repo for all types
/// Repository meets the requirements
/// </summary>
public interface ITodoListRepository
{
    Task CreateAsync(TodoList TodoList);
    Task UpdateAsync(TodoList TodoList);
    Task DeleteAsync(string id);
    Task<TodoList?> GetByIdAsync(string id);
    Task<List<TodoList>> GetUserTodoListsAsync(string userId, int page, int pageSize);
    Task<User> GetUserByIdAsync(string id);
    Task<User> CreateUser(User user);
}
