using CardService.Card;
using CardService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CardService.Tests
{
    public class CardControllerTests
    {
        private readonly CardController _controller;
        private readonly Mock<ICardService> _mockCardService;

        public CardControllerTests()
        {
            _mockCardService = new Mock<ICardService>();
            _controller = new CardController(_mockCardService.Object);
        }

        [Theory]
        [InlineData(null, "card1")]
        [InlineData("user1", null)]
        public async Task GetCardActions_ReturnsBadRequest_WhenUserIdOrCardNumberIsMissing(string userId, string cardNumber)
        {
            // Act
            var result = await _controller.GetCardActions(userId, cardNumber);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User ID and Card Number are required", badRequestResult.Value);
        }

        [Fact]
        public async Task GetCardActions_ReturnsNotFound_WhenCardDetailsAreNull()
        {
            // Arrange
            _mockCardService.Setup(service => service.GetCardDetails(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((CardDetails)null);

            var result = await _controller.GetCardActions("nonexistentUser", "nonexistentCard");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Card not found", notFoundResult.Value);
        }

        [Fact]
        public async Task GetCardActions_ReturnsExpectedActions_WhenCardDetailsAreValid()
        {
            var cardDetails = new CardDetails("user1", "card2", CardType.Credit, CardStatus.Blocked, true);
            _mockCardService.Setup(service => service.GetCardDetails("user1", "card2"))
                .ReturnsAsync(cardDetails);
            _mockCardService.Setup(service => service.GetActions(cardDetails))
                .Returns(new List<string> { "ACTION3", "ACTION4", "ACTION5", "ACTION6", "ACTION7", "ACTION8", "ACTION9" });

            var result = await _controller.GetCardActions("user1", "card2") as OkObjectResult;

            Assert.NotNull(result);
            var actions = Assert.IsType<List<string>>(result.Value);
            var expectedActions = new List<string> { "ACTION3", "ACTION4", "ACTION5", "ACTION6", "ACTION7", "ACTION8", "ACTION9" };
            Assert.Equal(expectedActions.Count, actions.Count);
            Assert.Equal(expectedActions, actions);
        }
    }
}
