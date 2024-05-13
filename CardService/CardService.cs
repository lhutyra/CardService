
using CardService.Card;

namespace CardService
{
    public class CardService
    {
        private readonly Dictionary<string, Dictionary<string, CardDetails>> _userCards;

        public CardService()
        {
            _userCards = GenerateSampleData();
        }

        private Dictionary<string, Dictionary<string, CardDetails>> GenerateSampleData()
        {
            var userCards = new Dictionary<string, Dictionary<string, CardDetails>>();

            for (var i = 1; i <= 3; i++)
            {
                var cards = new Dictionary<string, CardDetails>();
                var cardIndex = 1;
                foreach (CardType cardType in Enum.GetValues(typeof(CardType)))
                {
                    foreach (CardStatus cardStatus in Enum.GetValues(typeof(CardStatus)))
                    {
                        cards[$"card{cardIndex}"] = new CardDetails($"user{i}", $"card{cardIndex}", cardType, cardStatus, cardIndex % 2 == 0);
                        cardIndex++;
                    }
                }
                userCards[$"user{i}"] = cards;
            }

            return userCards;
        }

        public CardDetails GetCardDetails(string userId, string cardNumber)
        {
            if (_userCards.TryGetValue(userId, out var cards))
            {
                if (cards.TryGetValue(cardNumber, out var cardDetails))
                {
                    return cardDetails;
                }
            }

            return null;
        }

        public List<string> GetActions(CardDetails cardDetails)
        {
            var actions = new List<string>();

            var cardKey = (cardDetails.CardType, cardDetails.CardStatus);
            if (_cardActions.TryGetValue(cardKey, out var baseActions))
            {
                actions.AddRange(baseActions);
            }

            if (cardDetails.CardStatus == CardStatus.Blocked && cardDetails.IsPinSet)
            {
                actions.Add("ACTION6");
                actions.Add("ACTION7");
            }

            return actions;
        }

        private readonly Dictionary<(CardType, CardStatus), List<string>> _cardActions = new Dictionary<(CardType, CardStatus), List<string>>
    {
        { (CardType.Prepaid, CardStatus.Closed), new List<string> { "ACTION3", "ACTION4", "ACTION9" } },
        { (CardType.Credit, CardStatus.Blocked), new List<string> { "ACTION3", "ACTION4", "ACTION5", "ACTION8", "ACTION9" } },
    };
    }
}
