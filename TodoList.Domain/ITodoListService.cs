using TodoListApp.Entities.Models;

namespace TodoListApp.Domain;

public interface ITodoListService
{
    Task<TodoList> CreateTodoListAsync(string name);
    Task<List<TodoList>> GetUserTodoListsAsync(int page, int pageSize);
    Task<TodoList> GetTodoListAsync(string todoListId);
    Task UpdateTodoList(TodoList TodoList);
    Task DeleteTodoList(string id);
}