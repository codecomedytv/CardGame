using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Events;

namespace CardGame.Server.Game.Commands
{
    public class ModifyCard: GameEvent, ICommand
    {
        public readonly object Old;
        public readonly object New;
        public readonly string Property;
        public readonly ISource Source;
        public readonly Card Card;
        
        public ModifyCard(ISource source, Card card, string property, object newValue)
        {
            Source = source;
            Card = card;
            Old = card.Get(Property);
            Property = property;
            New = newValue;
        }

        public void Execute() => Card.Set(Property, New);

        public void Undo() => Card.Set(Property, Old);
    }
}
