namespace APIProductCatalog.Pagination
{
    public abstract class QueryStringParameters
    {
        const int maxPageSize = int.MaxValue;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = maxPageSize;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
