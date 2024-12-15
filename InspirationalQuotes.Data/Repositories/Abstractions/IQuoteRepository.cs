using InspirationalQuotes.Data.Models;

namespace InspirationalQuotes.Data.Repositories.Abstractions
{
    public interface IQuoteRepository
    {
        Task<Quote?> GetAsync(int id);
        Task<bool> CreateAsync(List<Quote> quotes);
        Task<bool> UpdateAsync(Quote quote);
        Task<bool> DeleteAsync(Quote quote);
        Task<List<Quote>> GetFilteredQuotesAsync(string filterName, string filterValue);
        //Task<List<string>> GetAllTagsAsync();
        Task<List<Quote>> GetAllQuotes(SearchQuote filter);
        //Task<List<Quote>> GetAll();
    }
}
