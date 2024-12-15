using InspirationalQuotes.UI.RequestModels;
using InspirationalQuotes.UI.ResponseModels;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;

namespace InspirationalQuotes.UI.Data
{
    public class QuoteService
    {
        private readonly HttpClient _httpClient;

        public QuoteService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri("https://localhost:7097/");
        }

        public async Task<List<QuoteRequest>> GetQuotes()
        {
            var response = await _httpClient.GetAsync("api/Quote/search");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<QuoteResponse>>(content);

            var quoteList = list.Select(q => new QuoteRequest
            {
                Id = q.Id,
                Author = q.Author,
                Tags = q.Tags?.Split(",").ToList(),
                QuoteDesp = q.QuoteDesp
            }).ToList();

            return quoteList;
        }

        public async Task<List<QuoteRequest>> GetQuotes(QuoteRequest quote)
        {
            var parameters = new List<KeyValuePair<string, string?>>()
            {
                new KeyValuePair<string, string?>("AuthorName", string.IsNullOrWhiteSpace(quote.Author) ? null : quote.Author),
                new KeyValuePair<string, string?>("Tag", quote.Tags?.Any() == true ? string.Join(", ", quote.Tags) : null),
                new KeyValuePair<string, string?>("Description", string.IsNullOrWhiteSpace(quote.QuoteDesp) ? null : quote.QuoteDesp),
            }.Where(kvp => kvp.Value != null).ToList();

            var queryString = QueryString.Create(parameters).ToString();
            var response = await _httpClient.GetAsync($"api/Quote/search{queryString}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var quotes = JsonConvert.DeserializeObject<List<QuoteResponse>>(content);

            var quoteList = quotes.Select(q => new QuoteRequest
            {
                Id = q.Id,
                Author = q.Author,
                Tags = q.Tags?.Split(",").ToList(),
                QuoteDesp = q.QuoteDesp
            }).ToList();

            return quoteList;
        }

        public async Task<string> AddQuotes(List<QuoteRequest> quotes)
        {
            var quotesList = quotes.Select(q => new QuoteResponse
            {
                Author = q.Author,
                Tags = q.Tags.Count > 0 ? string.Join(", ", q.Tags) : string.Empty,
                QuoteDesp = q.QuoteDesp
            }).ToList();

            var content = new StringContent(JsonConvert.SerializeObject(quotesList), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Quote/create", content);

            if (response.IsSuccessStatusCode)
            {
                return "Quotes created successfully";
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            return $"Failed to create quotes: {errorMessage}";
        }

        public async Task<string> DeleteQuote(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Quote/{id}");

            if (response.IsSuccessStatusCode)
            {
                return "Quote Deleted Successfully";
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            return $"Failed to Delete quote: {errorMessage}";
        }

        public async Task<QuoteRequest> GetQuoteById(int id)
        {
            var response = await _httpClient.GetAsync($"api/Quote/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var quoteResponse = JsonConvert.DeserializeObject<QuoteResponse>(content);

            var quote = new QuoteRequest
            {
                Id = quoteResponse.Id,
                Author = quoteResponse.Author,
                QuoteDesp = quoteResponse.QuoteDesp,
                Tags = quoteResponse.Tags?.Split(",").ToList()
            };

            return quote;
        }

        public async Task<string> UpdateQuote(QuoteRequest quoteRequest)
        {
            if (quoteRequest?.Id > 0)
            {
                var quote = new QuoteResponse
                {
                    Id = quoteRequest.Id,
                    Author = quoteRequest.Author,
                    QuoteDesp = quoteRequest.QuoteDesp,
                    Tags = quoteRequest.Tags != null && quoteRequest.Tags.Any()
                        ? string.Join(", ", quoteRequest.Tags)
                        : string.Empty
                };

                var content = new StringContent(JsonConvert.SerializeObject(quote), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/Quote/{quoteRequest.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return "Quote Updated Successfully";
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                return $"Failed to Update quote: {errorMessage}";
            }

            return "Failed to Update quote: Quote Id is not available";
        }
    }
}
