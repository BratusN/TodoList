using Microsoft.AspNetCore.Mvc;
using TodoListApp.Domain;

namespace TodoListApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShareController : ControllerBase
{
    private readonly IListShareService _shareService;

    public ShareController(IUserService userService,
        IListShareService shareService, ITodoListService todoService)
    {
        _shareService = shareService;
    }

    /// <summary>
    /// should be object as param
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ShareRequest shareRequest)
    {
         await _shareService.AddShareAsync(shareRequest.TodoListId,shareRequest.UserId);
        return Ok();
    }

    [HttpGet("todolist/{todoListId}/shares")]
    public async Task<IActionResult> GetShares(string todoListId)
    {
        var res = await _shareService.GetSharesForTodoListAsync(todoListId);
        return Ok(res);
    }  

    [HttpDelete("todolist/{todoListId}/shares/{userId}")]
    public async Task<IActionResult> DeleteTodoList(string todoListId, string userId)
    {
        await _shareService.DeleteShareAsync(todoListId, userId);
        return Ok();
    }

    public record ShareRequest( string TodoListId, string UserId);
}
