using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InspirationalQuotes.Data.DBContext;
using InspirationalQuotes.Data.Models;
using InspirationalQuotes.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace InspirationalQuotes.Data.Repositories.Implementations
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly QuoteDBContext _db;

        public QuoteRepository(QuoteDBContext db)


        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<bool> CreateAsync(List<Quote> quotes)
        {
            if (quotes == null || !quotes.Any()) return false;

            _db.Quotes.AddRange(quotes);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Quote quote)
        {
            if (quote == null) return false;

            _db.Quotes.Remove(quote);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Quote?> GetAsync(int id)
        {
            return await _db.Quotes.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(Quote quote)
        {
            if (quote == null) return false;

            _db.Update(quote);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Quote>> GetFilteredQuotesAsync(string filterName, string? filterValue)
        {
            if (string.IsNullOrWhiteSpace(filterValue))
                return new List<Quote>();

            IQueryable<Quote> filteredQuotes = _db.Quotes;

            filterValue = filterValue.ToLower();

            filteredQuotes = filterName.ToLower() switch
            {
                "author" => filteredQuotes.Where(q => q.Author.ToLower().Contains(filterValue)),
                "tag" => filteredQuotes.Where(q => q.Tags.ToLower().Contains(filterValue)),
                "desp" => filteredQuotes.Where(q => q.QuoteDesp.ToLower().Contains(filterValue)),
                "all" => filteredQuotes.Where(q =>
                    q.Author.ToLower().Contains(filterValue) ||
                    q.Tags.ToLower().Contains(filterValue) ||
                    q.QuoteDesp.ToLower().Contains(filterValue)),
                _ => filteredQuotes
            };

            return await filteredQuotes.Distinct().ToListAsync();
        }

        public async Task<List<Quote>> GetAllQuotes(SearchQuote filter)
        {
            try
            {
                var tags = (filter.Tag ?? string.Empty)
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => t.Trim().ToLower())
                    .ToList();

                var quotesQuery = _db.Quotes.AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.AuthorName))
                {
                    var authorName = filter.AuthorName.ToLower();
                    quotesQuery = quotesQuery.Where(q => q.Author.ToLower().Contains(authorName));
                }

                if (!string.IsNullOrWhiteSpace(filter.Description))
                {
                    var desp = filter.Description.ToLower();
                    quotesQuery = quotesQuery.Where(q => q.QuoteDesp.ToLower().Contains(desp));
                }

                var quotes = await quotesQuery.OrderBy(q => q.Id).ToListAsync();

                if (tags.Any())
                {
                    quotes = quotes.Where(q =>
                        q.Tags != null &&
                        q.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(t => t.Trim().ToLower())
                            .Any(t => tags.Contains(t)))
                        .ToList();
                }

                return quotes.Skip(filter.PageNumber * filter.PageSize)
                             .Take(filter.PageSize)
                             .ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
