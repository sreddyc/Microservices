namespace Application.Models
{
    public class ProductInquiryParams1
    {
        private const int MaxPageSize = 50;
        private string productNumber;
        public string ProductNumber { get { return this.productNumber; } set { this.productNumber = value?.Trim(); } }
        private string description;
        public string Description { get { return this.description; } set { this.description = value?.Trim(); } }
        public int CurrentPageNumber { get; set; }
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
        public string SortDirection { get; set; }
        public string SortExpression { get; set; }
    }
}