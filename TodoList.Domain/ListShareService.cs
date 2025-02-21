using Microsoft.Extensions.Logging;
using TodoListApp.Entities;

namespace TodoListApp.Domain;

public interface IListShareService
{
    public Task AddShareAsync(string todoListId, string userId);
    public Task DeleteShareAsync(string todoListId, string user);
    public Task<List<string>> GetSharesForTodoListAsync(string todoListId);
}
public class ListShareService : IListShareService
{
    private readonly ITodoListRepository _repository;
    private readonly IUserService _userService;
    private readonly ILogger<ListShareService> _logger;

    public ListShareService(ITodoListRepository repository, IUserService userService,
        ILogger<ListShareService> logger)
    {
        _repository = repository;
        _userService = userService;
        _logger = logger;
    }

    public async Task AddShareAsync(string todoListId, string userId)
    {
        //todo add more info to failure
        var todo = await _repository.GetByIdAsync(todoListId) ?? throw new ArgumentException(nameof(todoListId));
        var currentUser = await _userService.GetCurrentUser();
        if(!_userService.CanAccess(currentUser, todo, AccessType.Edit))
            throw new UnauthorizedAccessException();

        if(currentUser.Id == userId || todo.SharedWith.Contains(userId))
            return;

        var user = await _repository.GetUserByIdAsync(userId) ?? throw new ArgumentException(nameof(userId));
        todo.SharedWith.Add(userId);
        await _repository.UpdateAsync(todo);
    }


    public async Task DeleteShareAsync(string todoListId, string userId)
    {
        var todo = await _repository.GetByIdAsync(todoListId) ?? throw new ArgumentException(nameof(todoListId));
        var currentUser = await _userService.GetCurrentUser();
        if(!_userService.CanAccess(currentUser, todo, AccessType.Edit))
            throw new UnauthorizedAccessException();

        if(!todo.SharedWith.Contains(userId))
            return;

        todo.SharedWith.Remove(userId);
        await _repository.UpdateAsync(todo);
    }

    public async Task<List<string>> GetSharesForTodoListAsync(string todoListId)
    {
        var todo = await _repository.GetByIdAsync(todoListId) ?? throw new ArgumentException(nameof(todoListId));
        var currentUser = await _userService.GetCurrentUser();
        return !_userService.CanAccess(currentUser, todo, AccessType.View)
            ? throw new UnauthorizedAccessException()
            : todo.SharedWith;
    }

}
