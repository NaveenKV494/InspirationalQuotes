using InspirationalQuotes.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace InspirationalQuotes.Data.DBContext
{
    public class QuoteDBContext : DbContext
    {
        public QuoteDBContext(DbContextOptions<QuoteDBContext> options):base(options)
        {

        }
        public DbSet<Quote> Quotes { get; set; }
    }
}
