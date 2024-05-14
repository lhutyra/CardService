using System.Collections.Generic;
using CardService.Card;
using Xunit;

namespace CardService.Tests
{

    public class CardServiceTests
    {
        private readonly CardService _cardService;

        public CardServiceTests()
        {
            _cardService = new CardService();
        }

        [Theory]
        [InlineData("user1", "card1", CardType.Prepaid, CardStatus.Ordered, false)]
        [InlineData("user1", "card2", CardType.Prepaid, CardStatus.Inactive, true)]
        public async Task GetCardDetails_ShouldReturnCorrectCardDetails(string userId, string cardNumber, CardType expectedCardType, CardStatus expectedCardStatus, bool expectedIsPinSet)
        {
            // Act
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);

            // Assert
            Assert.NotNull(cardDetails);
            Assert.Equal(expectedCardType, cardDetails.CardType);
            Assert.Equal(expectedCardStatus, cardDetails.CardStatus);
            Assert.Equal(expectedIsPinSet, cardDetails.IsPinSet);
        }

        [Fact]
        public async Task GetCardDetails_ShouldReturnNull_WhenCardDoesNotExist()
        {
            // Act
            var cardDetails = await _cardService.GetCardDetails("user999", "card999");

            // Assert
            Assert.Null(cardDetails);
        }

        [Theory]
        [InlineData("user1", "card7", new[] { "ACTION3", "ACTION4", "ACTION9" })] // Prepaid Closed
        public async Task GetActions_ShouldReturnCorrectActions(string userId, string cardNumber, string[] expectedActions)
        {
            // Arrange
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);

            // Act
            var actions = _cardService.GetActions(cardDetails);

            // Assert
            Assert.Equal(expectedActions, actions);
        }

        [Fact]
        public async Task GetActions_ShouldReturnEmptyList_WhenCardDetailsAreNull()
        {
            // Arrange
            var cardDetails = new CardDetails("user1", "nonexistent", CardType.Prepaid, CardStatus.Closed, false);

            // Act
            var card = await _cardService.GetCardDetails("user1", "nonexisten");
            var actions = _cardService.GetActions(card);

            // Assert
            Assert.Empty(actions);
        }
    }



}