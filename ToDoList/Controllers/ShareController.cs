using Microsoft.AspNetCore.Mvc;
using TodoListApp.Domain;
using TodoListApp.Entities;

namespace TodoListApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShareController : ControllerBase
{
    private readonly IListShareService _shareService;
    private readonly TodoListServiceResponceProcessor _responceProcessor;

    public ShareController(IUserService userService,
        IListShareService shareService, ITodoListService todoService,
        TodoListServiceResponceProcessor responceProcessor)
    {
        _shareService = shareService;
        _responceProcessor = responceProcessor;
    }

    /// <summary>
    /// should be object as param
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ShareRequest shareRequest)
    {
        var res = await _shareService.AddShareAsync(shareRequest.TodoListId,shareRequest.UserId);
        return _responceProcessor.Process(res);
    }

    [HttpGet("todolist/{todoListId}/shares")]
    public async Task<IActionResult> GetShares(string todoListId)
    {
        var res = await _shareService.GetSharesForTodoListAsync(todoListId);
        return _responceProcessor.Process(res);
    }  

    [HttpDelete("todolist/{todoListId}/shares/{userId}")]
    public async Task<IActionResult> DeleteTodoList(string todoListId, string userId)
    {
        var res = await _shareService.DeleteShareAsync(todoListId, userId);
        return _responceProcessor.Process(res);
    }

    public record ShareRequest( string TodoListId, string UserId);
}
