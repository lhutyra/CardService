using Microsoft.VisualBasic;

namespace CardService.Controllers
{
    public class CardDetails
    {
        public string CardNumber { get; set; }
        public CardType CardType { get; set; }
        public CardStatus CardStatus { get; set; }
        public bool IsPinSet { get; set; }

        public CardDetails(string cardNumber, CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            CardNumber = cardNumber;
            CardType = cardType;
            CardStatus = cardStatus;
            IsPinSet = isPinSet;
        }
    }
}