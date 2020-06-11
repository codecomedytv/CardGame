using CardGame.Server.Game.Cards;
using Godot.Collections;

namespace CardGame.Server.Game.Commands
{
    public class UnreadyCard : GameEvent, ICommand
    {
        public readonly Card Card;

        public UnreadyCard(Card card)
        {
            Card = card;
            Message = new Network.Messages.UnreadyCard(card);
        }
        
        public void Execute() => Card.Ready = false;
        public void Undo() => Card.Ready = true;
        
    }
}