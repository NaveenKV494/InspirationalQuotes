using InspirationalQuotes.Data.Models;
using InspirationalQuotes.Data.Repositories.Abstractions;
using InspirationalQuotes.Service.Service.Abstractions;

namespace InspirationalQuotes.Service.Service.Implentations
{
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteRepository _quoteRepository;

        public QuoteService(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        public async Task<bool> Create(List<Quote> quotes)
        {
            if (quotes == null || !quotes.Any())
            {
                throw new ArgumentException("Quote list cannot be null or empty.");
            }
            return await _quoteRepository.CreateAsync(quotes);
        }

        public async Task<bool> Delete(int id)
        {
            var existingQuote = await _quoteRepository.GetAsync(id);
            if (existingQuote == null)
            {
                return false;
            }
            return await _quoteRepository.DeleteAsync(existingQuote);
        }

        public async Task<Quote> GetQuote(int id)
        {
            var quote = await _quoteRepository.GetAsync(id);
            if (quote == null)
            {
                throw new KeyNotFoundException($"Quote with ID {id} not found.");
            }
            return quote;
        }

        public async Task<bool> Update(Quote quote)
        {
            if (quote == null)
            {
                throw new ArgumentNullException(nameof(quote), "Quote cannot be null.");
            }

            var existingQuote = await _quoteRepository.GetAsync(quote.Id);
            if (existingQuote == null)
            {
                return false;
            }

            existingQuote.Author = quote.Author;
            existingQuote.Tags = quote.Tags;
            existingQuote.QuoteDesp = quote.QuoteDesp;

            return await _quoteRepository.UpdateAsync(existingQuote);
        }

        public async Task<List<Quote>> GetAll(SearchQuote filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter), "Search filter cannot be null.");
            }

            return await _quoteRepository.GetAllQuotes(filter);
        }
    }
}
