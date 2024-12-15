namespace InspirationalQuotes.UI.RequestModels
{
    public class QuoteRequest
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public List<string> Tags { get; set; }
        public string QuoteDesp { get; set; }
    }
}
