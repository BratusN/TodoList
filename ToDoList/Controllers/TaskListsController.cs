using Microsoft.AspNetCore.Mvc;
using TodoListApp.Domain;
using TodoListApp.Entities;
using TodoListApp.Entities.Models;

namespace TodoListApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoListsController : ControllerBase
{
    private readonly ITodoListService _todoService;
    private readonly IUserService _userService;

    public TodoListsController(ITodoListService service, IUserService userService)
    {
        _todoService = service;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodoList([FromBody] string name)
    {
        var res = await _todoService.CreateTodoListAsync(name);
        return Ok(res);
    }


    [HttpGet]
    public async Task<IActionResult> GetUserTodoLists([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var res = await _todoService.GetUserTodoListsAsync(page, pageSize);
        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodoList(string id)
    {
        var res = await _todoService.GetTodoListAsync(id);
        return Ok(res);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodoList(string id, TodoList todoList)
    {
        ArgumentNullException.ThrowIfNull(todoList);
        if(id != todoList.Id)
            return BadRequest($"{nameof(id)} must be equal to {nameof(TodoList)}.id");

        await _todoService.UpdateTodoList(todoList);
        return Ok(); 
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoList(string id)
    {
        await _todoService.DeleteTodoList(id);
        return Ok();
    }

}