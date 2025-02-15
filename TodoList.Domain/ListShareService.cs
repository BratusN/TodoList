using Microsoft.Extensions.Logging;
using TodoListApp.Domain.Respponse;
using TodoListApp.Entities;
using TodoListApp.Entities.Models;

namespace TodoListApp.Domain;

public interface IListShareService
{
    public Task<ServiceResponse> AddShareAsync(string task, string userId);
    public Task<ServiceResponse> DeleteShareAsync(string task, string user);
    public Task<ServiceResponse<List<string>>> GetSharesForTodoListAsync(string todoListId);
}
public class ListShareService : IListShareService
{
    private readonly ITodoListRepository _repository;
    private readonly IUserService _userService;
    private readonly IResponseBuilder _responseBuilder;
    private readonly IExceptionHandler _exceptionHandler;
    private readonly ILogger<ListShareService> _logger;

    public ListShareService(ITodoListRepository repository, IUserService userService,
        ILogger<ListShareService> logger, IResponseBuilder responseBuilder, IExceptionHandler exceptionHandler)
    {
        _repository = repository;
        _userService = userService;
        _logger = logger;
        _responseBuilder = responseBuilder;
        _exceptionHandler = exceptionHandler;
    }
    public async Task<ServiceResponse> AddShareAsync(string todoListId, string userId)
    {
        try
        {
            //todo add more info to failure
            var todo = await _repository.GetByIdAsync(todoListId);
            if(todo is null)
                return _responseBuilder.Failure<TodoList>(Errors.NotFound);
            var currentUser = await _userService.GetCurrentUser();
            if(!_userService.CanAccess(currentUser, todo, AccessType.Edit))
                return _responseBuilder.Failure(Errors.AccessDenied);

            if(currentUser.Id == userId || todo.SharedWith.Contains(userId))
                return _responseBuilder.Success();

            var user = await _repository.GetUserByIdAsync(userId);
            if(user is null)
                return _responseBuilder.Failure<TodoList>(Errors.NotFound);

            todo.SharedWith.Add(userId);
            await _repository.UpdateAsync(todo);
            return _responseBuilder.Success();
        }
        catch(Exception ex)
        {
            return _exceptionHandler.HandleException(ex, _logger);
        }
    }


    public async Task<ServiceResponse> DeleteShareAsync(string todoId, string userId)
    {
        try
        {
            var todo = await _repository.GetByIdAsync(todoId);
            if(todo is null)
                return _responseBuilder.Failure<TodoList>(Errors.NotFound);
            var currentUser = await _userService.GetCurrentUser();
            if(!_userService.CanAccess(currentUser, todo, AccessType.Edit))
                return _responseBuilder.Failure(Errors.AccessDenied);

            if(!todo.SharedWith.Contains(userId))
                return _responseBuilder.Success();

            todo.SharedWith.Remove(userId);
            await _repository.UpdateAsync(todo);
            return _responseBuilder.Success();
        }
        catch(Exception ex)
        {
            return _exceptionHandler.HandleException(ex, _logger);
        }
    }

    public async Task<ServiceResponse<List<string>>> GetSharesForTodoListAsync(string todoListId)
    {
        try
        {
            var todo = await _repository.GetByIdAsync(todoListId);
            if(todo is null)
                return _responseBuilder.Failure<List<string>>(Errors.NotFound);
            var currentUser = await _userService.GetCurrentUser();
            return !_userService.CanAccess(currentUser, todo, AccessType.View)
                ? _responseBuilder.Failure<List<string>>(Errors.AccessDenied)
                : _responseBuilder.Success(todo.SharedWith);
        }
        catch(Exception ex)
        {
            return _exceptionHandler.HandleException<List<string>>(ex, _logger);
        }
    }

}
