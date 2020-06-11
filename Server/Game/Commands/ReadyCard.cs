using CardGame.Server.Game.Cards;
using Godot.Collections;

namespace CardGame.Server.Game.Commands
{
    public class ReadyCard : GameEvent, ICommand
    {
        public readonly Card Card;

        public ReadyCard(Card card)
        {
            Card = card;
            Message = new Network.Messages.ReadyCard(card);
        }

        public void Execute() => Card.Ready = true;
        public void Undo() => Card.Ready = false;
        
    }
}