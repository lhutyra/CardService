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
        [InlineData("user1", "card1", CardType.Prepaid, CardStatus.Closed, false, new[] { "ACTION3", "ACTION4", "ACTION9" })]
        [InlineData("user1", "card2", CardType.Credit, CardStatus.Blocked, true, new[] { "ACTION3", "ACTION4", "ACTION5", "ACTION6", "ACTION7", "ACTION8", "ACTION9" })]
        [InlineData("user1", "card3", CardType.Credit, CardStatus.Blocked, false, new[] { "ACTION3", "ACTION4", "ACTION5", "ACTION8", "ACTION9" })]
        public void GetActions_ReturnsExpectedActions(string userId, string cardNumber, CardType cardType, CardStatus cardStatus, bool isPinSet, string[] expectedActions)
        {
            // Act
            var cardDetails = _cardService.GetCardDetails(userId, cardNumber);
            var result = _cardService.GetActions(cardDetails);

            // Assert
            Assert.Equal(expectedActions.Length, result.Count);
            foreach (var action in expectedActions)
            {
                Assert.Contains(action, result);
            }
        }
    }

}