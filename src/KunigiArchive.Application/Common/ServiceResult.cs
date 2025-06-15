namespace KunigiArchive.Application.Common;

public class ServiceResult
{
    public bool IsSuccess { get; }
    
    public string Message { get; }

    protected ServiceResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
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

public class ServiceResult<T> : ServiceResult
{
    public T? Value { get; }

    private ServiceResult(T? value, bool isSuccess, string message)
        : base(isSuccess, message)
    {
        Value = value;
    }

    public static ServiceResult<T> Success(T value)
    {
        return new ServiceResult<T>(value, true, string.Empty);
    }

    public new static ServiceResult<T> Failure(string message)
    {
        return new ServiceResult<T>(default, false, message);
    }
    
    public new static ServiceResult<T> Failure()
    {
        return new ServiceResult<T>(default, false, string.Empty);
    }
}