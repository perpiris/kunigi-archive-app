namespace KunigiArchive.Application.Common;

public class ServiceResult
{
    public bool IsSuccess { get; private set; }
    
    private ServiceResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    public static ServiceResult Success() => new ServiceResult(true);
    public static ServiceResult Failure() => new ServiceResult(false);
}