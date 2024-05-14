
using CardService.Card;

namespace CardService
{
    public class CardService : ICardService
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

        public async Task<CardDetails?> GetCardDetails(string userId, string cardNumber)
        {
            // At this point, we would typically make an HTTP call to an external service
            // to fetch the data. For this example we use generated sample data.
            await Task.Delay(1000);
            if (_userCards.TryGetValue(userId, out var cards)
            && cards.TryGetValue(cardNumber, out var cardDetails))
            {
                return cardDetails;
            }
            return null;
        }


        public List<string> GetActions(CardDetails cardDetails)
        {
            var actions = new List<string>();

            if (cardDetails == null)
            {
                return actions;
            }

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
            { (CardType.Prepaid, CardStatus.Ordered), new List<string> { "ACTION1", "ACTION2" } },
            { (CardType.Prepaid, CardStatus.Inactive), new List<string> { "ACTION1", "ACTION2" } },
            { (CardType.Prepaid, CardStatus.Active), new List<string> { "ACTION1", "ACTION2" } },
            { (CardType.Prepaid, CardStatus.Restricted), new List<string> { "ACTION3", "ACTION4" } },
            { (CardType.Prepaid, CardStatus.Blocked), new List<string> { "ACTION3", "ACTION4" } },
            { (CardType.Prepaid, CardStatus.Expired), new List<string> { "ACTION3", "ACTION4" } },
            { (CardType.Prepaid, CardStatus.Closed), new List<string> { "ACTION3", "ACTION4", "ACTION9" } },

            { (CardType.Debit, CardStatus.Ordered), new List<string> { "ACTION1", "ACTION2" } },
            { (CardType.Debit, CardStatus.Inactive), new List<string> { "ACTION1", "ACTION2" } },
            { (CardType.Debit, CardStatus.Active), new List<string> { "ACTION1", "ACTION2" } },
            { (CardType.Debit, CardStatus.Restricted), new List<string> { "ACTION3", "ACTION4" } },
            { (CardType.Debit, CardStatus.Blocked), new List<string> { "ACTION3", "ACTION4" } },
            { (CardType.Debit, CardStatus.Expired), new List<string> { "ACTION3", "ACTION4" } },
            { (CardType.Debit, CardStatus.Closed), new List<string> { "ACTION3", "ACTION4", "ACTION9" } },

            { (CardType.Credit, CardStatus.Ordered), new List<string> { "ACTION1", "ACTION2" } },
            { (CardType.Credit, CardStatus.Inactive), new List<string> { "ACTION1", "ACTION2" } },
            { (CardType.Credit, CardStatus.Active), new List<string> { "ACTION1", "ACTION2" } },
            { (CardType.Credit, CardStatus.Restricted), new List<string> { "ACTION3", "ACTION4" } },
            { (CardType.Credit, CardStatus.Blocked), new List<string> { "ACTION3", "ACTION4", "ACTION5", "ACTION8", "ACTION9" } },
            { (CardType.Credit, CardStatus.Expired), new List<string> { "ACTION3", "ACTION4" } },
            { (CardType.Credit, CardStatus.Closed), new List<string> { "ACTION3", "ACTION4", "ACTION9" } },
        };
    }
}
