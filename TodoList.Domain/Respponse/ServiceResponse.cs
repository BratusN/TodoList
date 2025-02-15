namespace TodoListApp.Domain.Respponse;

public class ServiceResponse
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
}

public class ServiceResponse<T> : ServiceResponse
{
    public T? Data { get; set; }

}


