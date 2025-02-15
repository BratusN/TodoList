using Microsoft.AspNetCore.Mvc;
using TodoListApp.Entities;
using TodoListApp.Entities.Models;

namespace TodoListApp.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class FillerController : ControllerBase
{
    private readonly ITodoListRepository _repository;

    public FillerController(ITodoListRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> FillData()
    {
        for (int i = 0;i<5; i++)
        {
            var user = new User { Name = $"user_{i}" };
            await _repository.CreateUser(user);
            for(int j = 0; j <= i; j++)
            {
                var todoList = new TodoList
                {
                    Name = $"list_{j}",
                    OwnerId = user.Id,
                    TodoItems = new List<TodoItem> { new TodoItem { Name = "item" } }
                };
                await _repository.CreateAsync(todoList);
            }  
        }
        return Ok();
    }
}
