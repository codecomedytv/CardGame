using CardGame.Server.Game.Cards;
using Godot;

namespace CardGame.Server.Game.Commands
{
    public class Modify: GameEvent, ICommand
    {
        public readonly object Old;
        public readonly object New;
        public readonly string Property;
        public readonly Card Card;
        
        public Modify(Card card, string property, object newValue)
        {
            Card = card;
            Old = card.Get(Property);
            Property = property;
            New = newValue;
        }

        public void Execute() => Card.Set(Property, New);

        public void Undo() => Card.Set(Property, Old);
    }
}
