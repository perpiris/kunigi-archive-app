namespace KunigiArchive.Application.Common;

public class ServiceResult
{
    public bool IsSuccess { get; }
    public string ErrorMessage { get; }

    private ServiceResult(bool isSuccess, string errorMessage)
    {
        if (isSuccess && !string.IsNullOrEmpty(errorMessage))
        {
            throw new InvalidOperationException("A successful result cannot have an error message.");
        }

        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static ServiceResult Success()
    {
        return new ServiceResult(true, string.Empty);
    }
    
    public static ServiceResult Failure()
    {
        return new ServiceResult(false, string.Empty);
    }

    public static ServiceResult Failure(string message)
    {
        return new ServiceResult(false, message);
    }
}