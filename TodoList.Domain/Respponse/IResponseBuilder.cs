namespace TodoListApp.Domain.Respponse;

public interface IResponseBuilder
{
    ServiceResponse Success(string? message = null);
    ServiceResponse Failure(string errorMessage);
    ServiceResponse<T> Success<T>(T data, string? message = null);
    ServiceResponse<T> Failure<T>(string errorMessage);

}


