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
        [InlineData(CardType.Prepaid, CardStatus.Closed, false, new[] { "ACTION3", "ACTION4", "ACTION9" })]
        [InlineData(CardType.Credit, CardStatus.Blocked, true, new[] { "ACTION3", "ACTION4", "ACTION5", "ACTION6", "ACTION7", "ACTION8", "ACTION9" })]
        [InlineData(CardType.Credit, CardStatus.Blocked, false, new[] { "ACTION3", "ACTION4", "ACTION5", "ACTION8", "ACTION9" })]
        public void GetActions_ReturnsExpectedActions(CardType cardType, CardStatus cardStatus, bool isPinSet, string[] expectedActions)
        {
            // Act
            var result = _cardService.GetActions(cardType, cardStatus, isPinSet);

            // Assert
            Assert.Equal(expectedActions.Length, result.Count);
            foreach (var action in expectedActions)
            {
                Assert.Contains(action, result);
            }
        }
    }
}