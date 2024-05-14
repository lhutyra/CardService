namespace CardService.Card
{

    public interface ICardService
    {
        Task<CardDetails?> GetCardDetails(string userId, string cardNumber);
        List<string> GetActions(CardDetails cardDetails);
    }

}
