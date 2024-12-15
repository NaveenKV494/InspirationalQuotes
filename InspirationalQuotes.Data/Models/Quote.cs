using System.ComponentModel.DataAnnotations;

namespace InspirationalQuotes.Data.Models
{
    public class Quote
    {
        [Key]
        public int Id { get; set; }
        public string Author { get; set; }
        public string Tags {  get; set; }
        public string QuoteDesp { get; set; }
    }
}
