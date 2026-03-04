namespace LineageMOps.Models.ViewModels;

public class PaginatedList<T>
{
    public List<T> Items { get; set; } = new();
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public static PaginatedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize)
    {
        var list = source.ToList();
        var count = list.Count;
        var items = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<T>
        {
            Items = items,
            PageIndex = pageIndex,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize),
            TotalCount = count,
            PageSize = pageSize
        };
    }
}
