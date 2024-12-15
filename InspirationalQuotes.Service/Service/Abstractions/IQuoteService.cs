using InspirationalQuotes.Data.Models;

namespace InspirationalQuotes.Service.Service.Abstractions
{
    public interface IQuoteService
    {
        Task<bool> Create(List<Quote> quote);
        Task<bool> Update(Quote quote);
        Task<bool> Delete(int id);
        Task<Quote> GetQuote(int id);
        Task<List<Quote>> GetAll(SearchQuote filter);
    }
}
