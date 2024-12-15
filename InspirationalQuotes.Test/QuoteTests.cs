using InspirationalQuotes.Data.Models;
using InspirationalQuotes.Data.Repositories.Abstractions;
using InspirationalQuotes.Service.Service.Abstractions;
using InspirationalQuotes.API.Services.Endpoint;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace InspirationalQuotes_Test
{
    [TestFixture]
    public class QuoteTests
    {
        private Mock<IQuoteService> _quoteServiceMock;
        private QuoteController _quoteController;

        [SetUp]
        public void Setup()
        {
            _quoteServiceMock = new Mock<IQuoteService>();
            _quoteController = new QuoteController(_quoteServiceMock.Object);
        }

        #region GetQuote Tests

        [Test]
        public async Task GetQuote_ValidId_ReturnsOkResponse()
        {
            // Arrange
            var quoteId = 1;
            var quote = new Quote { Id = quoteId, Author = "John Doe", QuoteDesp = "Inspiration", Tags = "Motivation" };
            _quoteServiceMock.Setup(service => service.GetQuote(quoteId)).ReturnsAsync(quote);

            // Act
            var result = await _quoteController.GetQuote(quoteId);

            // Assert
            var okResult = result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
            ClassicAssert.AreEqual(quote, okResult.Value);
        }

        [Test]
        public async Task GetQuote_InvalidId_ReturnsNotFoundResponse()
        {
            // Arrange
            var invalidId = 999;
            _quoteServiceMock.Setup(service => service.GetQuote(invalidId)).ReturnsAsync((Quote)null);

            // Act
            var result = await _quoteController.GetQuote(invalidId);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            ClassicAssert.IsNotNull(notFoundResult);
            ClassicAssert.AreEqual((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Test]
        public async Task GetQuote_ThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var quoteId = 1;
            _quoteServiceMock.Setup(service => service.GetQuote(quoteId)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _quoteController.GetQuote(quoteId);

            // Assert
            var errorResult = result as ObjectResult;
            ClassicAssert.IsNotNull(errorResult);
            ClassicAssert.AreEqual((int)HttpStatusCode.InternalServerError, errorResult.StatusCode);
        }

        #endregion

        #region CreateQuotes Tests

        [Test]
        public async Task CreateQuotes_ValidData_ReturnsOkResponse()
        {
            // Arrange
            var quotes = new List<Quote>
            {
                new Quote { Id = 1, Author = "Author 1", Tags = "Tag 1", QuoteDesp = "Description 1" },
                new Quote { Id = 2, Author = "Author 2", Tags = "Tag 2", QuoteDesp = "Description 2" }
            };
            _quoteServiceMock.Setup(service => service.Create(quotes)).ReturnsAsync(true);

            // Act
            var result = await _quoteController.CreateQuotes(quotes);

            // Assert
            var okResult = result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Test]
        public async Task CreateQuotes_NullQuotes_ReturnsBadRequest()
        {
            // Arrange
            List<Quote> quotes = null;

            // Act
            var result = await _quoteController.CreateQuotes(quotes);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            ClassicAssert.IsNotNull(badRequestResult);
            ClassicAssert.AreEqual((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
        }

        [Test]
        public async Task CreateQuotes_EmptyQuotes_ReturnsBadRequest()
        {
            // Arrange
            var quotes = new List<Quote>();

            // Act
            var result = await _quoteController.CreateQuotes(quotes);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            ClassicAssert.IsNotNull(badRequestResult);
            ClassicAssert.AreEqual((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
        }

        [Test]
        public async Task CreateQuotes_ServiceFailure_ReturnsInternalServerError()
        {
            // Arrange
            var quotes = new List<Quote>
            {
                new Quote { Id = 1, Author = "Author 1", Tags = "Tag 1", QuoteDesp = "Description 1" }
            };
            _quoteServiceMock.Setup(service => service.Create(quotes)).ReturnsAsync(false);

            // Act
            var result = await _quoteController.CreateQuotes(quotes);

            // Assert
            var errorResult = result as ObjectResult;
            ClassicAssert.IsNotNull(errorResult);
            ClassicAssert.AreEqual((int)HttpStatusCode.InternalServerError, errorResult.StatusCode);
        }

        #endregion

        #region UpdateQuote Tests

        [Test]
        public async Task UpdateQuote_ValidData_ReturnsOkResponse()
        {
            // Arrange
            var quote = new Quote { Id = 1, Author = "Updated Author", QuoteDesp = "Updated Description", Tags = "Updated Tags" };
            _quoteServiceMock.Setup(service => service.Update(quote)).ReturnsAsync(true);

            // Act
            var result = await _quoteController.UpdateQuote(quote);

            // Assert
            var okResult = result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Test]
        public async Task UpdateQuote_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            Quote quote = null;

            // Act
            var result = await _quoteController.UpdateQuote(quote);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            ClassicAssert.IsNotNull(badRequestResult);
            ClassicAssert.AreEqual((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
        }

        #endregion

        #region DeleteQuote Tests

        [Test]
        public async Task DeleteQuote_ValidId_ReturnsOkResponse()
        {
            // Arrange
            var quoteId = 1;
            _quoteServiceMock.Setup(service => service.Delete(quoteId)).ReturnsAsync(true);

            // Act
            var result = await _quoteController.DeleteQuote(quoteId);

            // Assert
            var okResult = result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Test]
        public async Task DeleteQuote_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidId = 999;
            _quoteServiceMock.Setup(service => service.Delete(invalidId)).ReturnsAsync(false);

            // Act
            var result = await _quoteController.DeleteQuote(invalidId);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            ClassicAssert.IsNotNull(notFoundResult);
            ClassicAssert.AreEqual((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        #endregion

        #region SearchQuotes Tests

        [Test]
        public async Task SearchQuotes_ValidQuery_ReturnsOkResponse()
        {
            // Arrange
            var query = new SearchQuote
            {
                Description = "Inspiration" 
            };
            var quotes = new List<Quote>
            {
                new Quote { Id = 1, Author = "Author 1", QuoteDesp = "Inspiration", Tags = "Motivation" }
            };
            _quoteServiceMock.Setup(service => service.GetAll(query)).ReturnsAsync(quotes);

            // Act
            var result = await _quoteController.Search(query);

            // Assert
            var okResult = result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        #endregion
    }
}
