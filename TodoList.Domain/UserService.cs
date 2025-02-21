using Microsoft.AspNetCore.Http;
using TodoListApp.Entities;
using TodoListApp.Entities.Models;

namespace TodoListApp.Domain;

/// <summary>
/// Ideally 2 seperate services to implement single responsibility
/// </summary>
public class UserService : IUserService
{
    private readonly ITodoListRepository _repository;
    private readonly IHttpContextAccessor _contextAccessor;

    public UserService(ITodoListRepository TodoListRepository, IHttpContextAccessor contextAccessor)
    {
        _repository = TodoListRepository;
        _contextAccessor = contextAccessor;
    }
    //good enough for this exercise
    public async Task<User> GetCurrentUser()
    {
        if(_contextAccessor.HttpContext is null
            || !_contextAccessor.HttpContext.Request.Headers.TryGetValue("UserId", out var userId))
            throw new UnauthorizedAccessException();
        if(string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("invalid user Id");

        var user = await _repository.GetUserByIdAsync(userId);
        return user is null
            ? throw new ArgumentException("User not found")
            : user;
    }

    public bool CanAccess(User user, TodoList todo, AccessType access = AccessType.View)
    {
        return user != null
            && todo != null
            && (todo.OwnerId == user.Id
                || (todo.SharedWith.Contains(user.Id)
                    && (access == AccessType.View || access == AccessType.Edit)));
    }
}