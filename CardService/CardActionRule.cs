using CardService.Card;

namespace CardService
{
    public class CardActionRule
    {
        public Func<CardDetails, bool> Condition { get; }
        public List<string> Actions { get; }

        public CardActionRule(Func<CardDetails, bool> condition, List<string> actions)
        {
            Condition = condition;
            Actions = actions;
        }
    }
}
