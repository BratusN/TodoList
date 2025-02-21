using Microsoft.Extensions.Logging;
using TodoListApp.Entities;
using TodoListApp.Entities.Models;

namespace TodoListApp.Domain;

public class TodoListService : ITodoListService
{
    private readonly ITodoListRepository _repository;
    private readonly IUserService _userService;
    private readonly ILogger<TodoListService> _logger;

    public TodoListService(ITodoListRepository repository, IUserService userService,
        ILogger<TodoListService> logger)
    {
        _repository = repository;
        _userService = userService;
        _logger = logger;
    }

    public async Task<TodoList> CreateTodoListAsync(string name)
    {
        if(string.IsNullOrWhiteSpace(name) || name.Length > 255)
            throw new ArgumentException("Invalid todolist name");

        var user = await _userService.GetCurrentUser();
        if(user is null) throw new UnauthorizedAccessException();

        var todo = new TodoList
        {
            Name = name,
            OwnerId = user.Id,
        };

        await _repository.CreateAsync(todo);
        return todo;
    }

    public async Task<List<TodoList>> GetUserTodoListsAsync(int page, int pageSize)
    {

        var user = await _userService.GetCurrentUser();
        if(user is null)
            throw new UnauthorizedAccessException();

        var lists = await _repository.GetUserTodoListsAsync(user.Id, page, pageSize);
        return lists;
    }


    public async Task DeleteTodoList(string id)
    {
        var user = await _userService.GetCurrentUser();
        if(user is null)
            throw new UnauthorizedAccessException();
        var todo = await _repository.GetByIdAsync(id);
        if(todo == null)
            throw new ArgumentException("Todolist not found");
        if(!_userService.CanAccess(user, todo, AccessType.Delete))
            throw new UnauthorizedAccessException();
        await _repository.DeleteAsync(id);

    }

    public async Task<TodoList> GetTodoListAsync(string todoListId)
    {
        var user = await _userService.GetCurrentUser();
        if(user is null)
            throw new UnauthorizedAccessException();

        var todo = await _repository.GetByIdAsync(todoListId);
        return todo == null
            ? throw new ArgumentException("TodoList not found")
            : _userService.CanAccess(user, todo)
           ? todo
           : throw new UnauthorizedAccessException();
    }

    public async Task UpdateTodoList(TodoList todoList)
    {

        ArgumentNullException.ThrowIfNull(todoList);

        var user = await _userService.GetCurrentUser();
        if(user is null)
            throw new UnauthorizedAccessException();

        var todo = await _repository.GetByIdAsync(todoList.Id);
        if(todo == null)
            throw new ArgumentException("TodoList not found");
        if(!_userService.CanAccess(user, todo, AccessType.Edit))
            throw new UnauthorizedAccessException();

        await _repository.UpdateAsync(todoList);
    }

}