namespace Wee_XYYY.Model;

public class PaginatedViewModel<T>
{
    public int PageIndex { get; private set; }
 
    public int PageSize { get; private set; }
 
    public long Count { get; private set; }
 
    public IEnumerable<T> Data { get; private set; }
 
    public PaginatedViewModel(int pageIndex, int pageSize, long count, IEnumerable<T> data)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Count = count;
        Data = data;
    }
}
