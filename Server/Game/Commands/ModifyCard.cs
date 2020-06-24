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
            // This may not work the way I intend it too.
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
