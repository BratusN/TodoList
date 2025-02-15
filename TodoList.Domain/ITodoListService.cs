using TodoListApp.Domain.Respponse;
using TodoListApp.Entities.Models;

namespace TodoListApp.Domain;

public interface ITodoListService
{
    Task<ServiceResponse<TodoList>> CreateTodoListAsync(string name);
    Task<ServiceResponse<List<TodoList>>> GetUserTodoListsAsync(int page, int pageSize);
    Task<ServiceResponse<TodoList>> GetTodoListAsync(string todoListId);
    Task<ServiceResponse> UpdateTodoList(TodoList TodoList);
    Task<ServiceResponse> DeleteTodoList(string id);
}