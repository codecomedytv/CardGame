using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Events
{
    public class ModifyCard: Event
    {
        public readonly object Old;
        public readonly object New;
        public readonly string Property;
        public readonly ISource Source;
        public readonly Card Card;
        
        public ModifyCard(ISource source, Card card, string property, object newValue)
        {
            // This may not work the way I intend it too.
            Source = source;
            Card = card;
            Old = card.Get(Property);
            Property = property;
            New = newValue;
        }

    }
}
