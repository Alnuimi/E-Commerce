using System;

namespace API.RequestHelper;

public class Pagination<T>(int _pageIndex,int _pageSize,int _count,IReadOnlyList<T> _data)
{
    public int PageIndex { get; set; }=_pageIndex;
    public int PageSize { get; set; }=_pageSize;
    public int Count { get; set; }=_count;
    public IReadOnlyList<T> Data { get; set; }=_data;
}
