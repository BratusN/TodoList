using TodoListApp.Entities.Models;

namespace TodoListApp.Domain;
public interface IUserService
{
    Task<User> GetCurrentUser();
    bool CanAccess(User user, TodoList todo, AccessType access = AccessType.View);

}
public enum AccessType
{
    View = 0,
    Edit = 1,
    Delete = 2
}