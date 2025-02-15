using Microsoft.Extensions.Logging;
using TodoListApp.Domain.Respponse;

namespace TodoListApp.Domain;
public interface IExceptionHandler
{
    ServiceResponse<T> HandleException<T>(Exception ex, ILogger logger);
    ServiceResponse HandleException(Exception ex, ILogger logger);
}

public class ExceptionHandler : IExceptionHandler
{
    private readonly IResponseBuilder _responseBuilder;

    public ExceptionHandler(IResponseBuilder responseBuilder)
    {
        _responseBuilder = responseBuilder;
    }

    public ServiceResponse<T> HandleException<T>(Exception ex, ILogger logger)
    {
        logger.LogError(ex, "Failed");
        return _responseBuilder.Failure<T>(Errors.GeneralError);
    }
    public ServiceResponse HandleException(Exception ex, ILogger logger)
    {
        logger.LogError(ex, "Failed");
        return _responseBuilder.Failure(Errors.GeneralError);
    }
}
