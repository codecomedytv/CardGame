using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Commands
{
    public class ModifyCard: Command
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

        public override void Execute() => Card.Set(Property, New);

        public override void Undo() => Card.Set(Property, Old);
    }
}
