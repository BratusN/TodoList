using Microsoft.AspNetCore.Mvc;
using TodoListApp.Domain.Respponse;
namespace TodoListApp.Api;

public class TodoListServiceResponceProcessor
{
    public IActionResult Process<T>(ServiceResponse<T> res)
    {
        return res.IsSuccess ? new OkObjectResult(res.Data) :  Errror(res);
    }

    public IActionResult Process(ServiceResponse res)
    {
        return res.IsSuccess ? new OkResult(): Errror(res);      
    }

    private static IActionResult Errror(ServiceResponse res)
    {
        return new BadRequestObjectResult(res.Message);
    }
}
