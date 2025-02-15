namespace TodoListApp.Domain.Respponse;

public class ResponseBuilder : IResponseBuilder
{
    public ServiceResponse Success(string? message = null)
    {
        return new ServiceResponse { IsSuccess = true, Message = message };
    }

    public ServiceResponse Failure(string errorMessage)
    {
        return new ServiceResponse { IsSuccess = false, Message = errorMessage };
    }
    public ServiceResponse<T> Success<T>(T data, string? message = null)
    {
        return new ServiceResponse<T> { Data = data, IsSuccess = true, Message = message };
    }

    public ServiceResponse<T> Failure<T>(string errorMessage)
    {
        return new ServiceResponse<T> { IsSuccess = false, Message = errorMessage };
    }
}


