namespace KunigiArchive.Contracts.Common;

public class PaginatedResponse<T>
{
    public required List<T> Items { get; set; }
    
    public int CurrentPage { get; set; }
    
    public int TotalPages { get; set; }
    
    public int PageSize { get; set; }
}