namespace InspirationalQuotes.Data.Models
{
    public class SearchQuote
    {
        public string? AuthorName { get; set; }
        public string? Tag { get; set; }
        public string? Description { get; set; }
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
