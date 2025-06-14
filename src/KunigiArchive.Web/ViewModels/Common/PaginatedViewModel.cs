namespace KunigiArchive.Web.ViewModels.Common;

public class PaginatedViewModel<T>
{
    public required List<T> Items { get; set; }
    
    public int CurrentPage { get; set; }
    
    public int TotalPages { get; set; }
    
    public int PageSize { get; set; }
}