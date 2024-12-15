using InspirationalQuotes.Data.Models;
using InspirationalQuotes.Data.Repositories.Abstractions;
using InspirationalQuotes.Service.Service.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace InspirationalQuotes.API.Services.Endpoint
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuoteController : ControllerBase
    {
        private readonly IQuoteService _quoteService;

        public QuoteController(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Quote), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetQuote(int id)
        {
            try
            {
                var quote = await _quoteService.GetQuote(id);
                if (quote == null)
                {
                    return NotFound(new { Message = $"Quote with ID {id} not found." });
                }
                return Ok(quote);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Error = ex.Message });
            }
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateQuotes([FromBody] List<Quote> quotes)
        {
            try
            {
                if (quotes == null || !quotes.Any())
                {
                    return BadRequest(new { Message = "No quotes provided for creation." });
                }

                var isSuccess = await _quoteService.Create(quotes);
                if (isSuccess)
                {
                    return Ok(new { Message = "Quotes created successfully." });
                }
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = "Failed to create quotes." });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateQuote([FromBody] Quote quote)
        {
            try
            {
                if (quote == null)
                {
                    return BadRequest(new { Message = "Invalid data provided for update." });
                }

                var isSuccess = await _quoteService.Update(quote);
                if (isSuccess)
                {
                    return Ok(new { Message = "Quote updated successfully." });
                }
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = "Failed to update quote." });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            try
            {
                var isSuccess = await _quoteService.Delete(id);
                if (!isSuccess)
                {
                    return NotFound(new { Message = $"Quote with ID {id} not found. Deletion failed." });
                }
                return Ok(new { Message = "Quote deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Error = ex.Message });
            }
        }

        [HttpGet("search")]
        //[ProducesResponseType(typeof(IEnumerable<Quote>), (int)HttpStatusCode.OK)]
        //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Search([FromQuery] SearchQuote filter)
        {
            try
            {
                var response = await _quoteService.GetAll(filter);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Error = "Failed to get data.", Details = ex.Message });
            }
        }
    }
}
