
using CardService.Card;

namespace CardService
{
    public class CardService : ICardService
    {
        private readonly Dictionary<string, Dictionary<string, CardDetails>> _userCards;
        private readonly List<CardActionRule> _cardActionRules;

        public CardService()
        {
            _userCards = GenerateSampleData();
            _cardActionRules = GenerateCardActionRules();
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
            if (cardDetails == null)
            {
                return new List<string>();
            }

            return _cardActionRules
                .Where(rule => rule.Condition(cardDetails))
                .SelectMany(rule => rule.Actions)
                .Distinct()
                .ToList();
        }

        private List<CardActionRule> GenerateCardActionRules()
        {
            return new List<CardActionRule>
            {
                new CardActionRule(
                    cardDetails => cardDetails.CardType == CardType.Prepaid && cardDetails.CardStatus == CardStatus.Closed,
                    new List<string> { "ACTION3", "ACTION4", "ACTION9" }
                ),
                new CardActionRule(
                    cardDetails => cardDetails.CardType == CardType.Credit && cardDetails.CardStatus == CardStatus.Blocked,
                    new List<string> { "ACTION3", "ACTION4", "ACTION5", "ACTION8", "ACTION9" }
                ),
                new CardActionRule(
                    cardDetails => cardDetails.CardType == CardType.Prepaid && cardDetails.CardStatus == CardStatus.Ordered,
                    new List<string> { "ACTION1", "ACTION2" }
                ),
                new CardActionRule(
                    cardDetails => cardDetails.CardType == CardType.Prepaid && cardDetails.CardStatus == CardStatus.Inactive,
                    new List<string> { "ACTION1", "ACTION2" }
                ),
                new CardActionRule(
                    cardDetails => cardDetails.CardType == CardType.Prepaid && cardDetails.CardStatus == CardStatus.Active,
                    new List<string> { "ACTION1", "ACTION2" }
                ),
                new CardActionRule(
                    cardDetails => cardDetails.CardType == CardType.Prepaid &&
                                   (cardDetails.CardStatus == CardStatus.Restricted || cardDetails.CardStatus == CardStatus.Blocked || cardDetails.CardStatus == CardStatus.Expired),
                    new List<string> { "ACTION3", "ACTION4" }
                ),
                new CardActionRule(
                    cardDetails => cardDetails.CardType == CardType.Debit &&
                                   (cardDetails.CardStatus == CardStatus.Ordered || cardDetails.CardStatus == CardStatus.Inactive || cardDetails.CardStatus == CardStatus.Active),
                    new List<string> { "ACTION1", "ACTION2" }
                ),
                new CardActionRule(
                    cardDetails => cardDetails.CardType == CardType.Debit &&
                                   (cardDetails.CardStatus == CardStatus.Restricted || cardDetails.CardStatus == CardStatus.Blocked || cardDetails.CardStatus == CardStatus.Expired),
                    new List<string> { "ACTION3", "ACTION4" }
                ),
                new CardActionRule(
                    cardDetails => cardDetails.CardType == CardType.Debit && cardDetails.CardStatus == CardStatus.Closed,
                    new List<string> { "ACTION3", "ACTION4", "ACTION9" }
                ),
                new CardActionRule(
                    cardDetails => cardDetails.CardType == CardType.Credit &&
                                   (cardDetails.CardStatus == CardStatus.Ordered || cardDetails.CardStatus == CardStatus.Inactive || cardDetails.CardStatus == CardStatus.Active),
                    new List<string> { "ACTION1", "ACTION2" }
                ),
                new CardActionRule(
                    cardDetails => cardDetails.CardType == CardType.Credit &&
                                   (cardDetails.CardStatus == CardStatus.Restricted || cardDetails.CardStatus == CardStatus.Expired),
                    new List<string> { "ACTION3", "ACTION4" }
                ),
                new CardActionRule(
                    cardDetails => cardDetails.CardType == CardType.Credit && cardDetails.CardStatus == CardStatus.Closed,
                    new List<string> { "ACTION3", "ACTION4", "ACTION9" }
                ),
                new CardActionRule(
                    cardDetails => cardDetails.CardStatus == CardStatus.Blocked && cardDetails.IsPinSet,
                    new List<string> { "ACTION6", "ACTION7" }
                ),
            };
        }
    }

}
