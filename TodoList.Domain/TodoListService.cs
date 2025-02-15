using Microsoft.Extensions.Logging;
using TodoListApp.Domain.Respponse;
using TodoListApp.Entities;
using TodoListApp.Entities.Models;

namespace TodoListApp.Domain;

public class TodoListService : ITodoListService
{
    private readonly ITodoListRepository _repository;
    private readonly IUserService _userService;
    private readonly IResponseBuilder _responseBuilder;
    private readonly IExceptionHandler _exceptionHandler;
    private readonly ILogger<TodoListService> _logger;

    public TodoListService(ITodoListRepository repository, IUserService userService,
        ILogger<TodoListService> logger, IResponseBuilder responseBuilder, IExceptionHandler exceptionHandler)
    {
        _repository = repository;
        _userService = userService;
        _logger = logger;
        _responseBuilder = responseBuilder;
        _exceptionHandler = exceptionHandler;
    }

    public async Task<ServiceResponse<TodoList>> CreateTodoListAsync(string name)
    {
        try
        {
            if(string.IsNullOrWhiteSpace(name) || name.Length > 255)
                return
                    _responseBuilder.Failure<TodoList>(Errors.InvalidTodoListName);

            var user = await _userService.GetCurrentUser();
            if(user is null) return _responseBuilder.Failure<TodoList>(Errors.Unautorized);

            var todo = new TodoList
            {
                Name = name,
                OwnerId = user.Id,
            };

            await _repository.CreateAsync(todo);
            return _responseBuilder.Success(todo);
        }
        catch(Exception ex)
        {
            return _exceptionHandler.HandleException<TodoList>(ex, _logger);
        }
    }

    public async Task<ServiceResponse<List<TodoList>>> GetUserTodoListsAsync(int page, int pageSize)
    {
        try
        {
            var user = await _userService.GetCurrentUser();
            if(user is null)
                return _responseBuilder.Failure<List<TodoList>>(Errors.Unautorized);

            var lists = await _repository.GetUserTodoListsAsync(user.Id, page, pageSize);
            return _responseBuilder.Success(lists);
        }
        catch(Exception ex)
        {
            return _exceptionHandler.HandleException<List<TodoList>>(ex, _logger);
        }
    }


    public async Task<ServiceResponse> DeleteTodoList(string id)
    {
        try
        {
            var user = await _userService.GetCurrentUser();
            if(user is null)
                return _responseBuilder.Failure(Errors.Unautorized);
            var todo = await _repository.GetByIdAsync(id);
            if(todo == null)
                return _responseBuilder.Failure(Errors.NotFound);
            if(!_userService.CanAccess(user, todo, AccessType.Delete))
                return _responseBuilder.Failure(Errors.Unautorized);
            await _repository.DeleteAsync(id);
            return _responseBuilder.Success();
        }
        catch(Exception ex)
        {
            return _exceptionHandler.HandleException(ex, _logger);
        }
    }

    public async Task<ServiceResponse<TodoList>> GetTodoListAsync(string todoListId)
    {
        try
        {
            var user = await _userService.GetCurrentUser();
            if(user is null)
                return _responseBuilder.Failure<TodoList>(Errors.Unautorized);

            var todo = await _repository.GetByIdAsync(todoListId);
            if(todo == null)
                return _responseBuilder.Failure<TodoList>(Errors.NotFound);
             return   _userService.CanAccess(user, todo) 
                ? _responseBuilder.Success(todo) 
                : _responseBuilder.Failure<TodoList>(Errors.Unautorized);
        }
        catch(Exception ex)
        {
            return _exceptionHandler.HandleException<TodoList>(ex, _logger);
        }
    }

    public async Task<ServiceResponse> UpdateTodoList(TodoList todoList)
    {
        try
        {
            if(todoList is null)
                return _responseBuilder.Failure(Errors.ArgumentNull);

            var user = await _userService.GetCurrentUser();
            if(user is null)
                return _responseBuilder.Failure(Errors.Unautorized);

            var todo = await _repository.GetByIdAsync(todoList.Id);
            if(todo == null)
                return _responseBuilder.Failure(Errors.NotFound);
            if(!_userService.CanAccess(user, todo, AccessType.Edit))
                return _responseBuilder.Failure(Errors.Unautorized);

            await _repository.UpdateAsync(todoList);
            return _responseBuilder.Success();
        }
        catch(Exception ex)
        {
            return _exceptionHandler.HandleException(ex, _logger);
        }
    }

}