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
        }
        
        public void Execute()
        {
            Card.Ready = false;
        }

        public void Undo()
        {
            Card.Ready = true;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.UnreadyCard;
            message.Player["args"] = new Array{Card.Id};
            message.Opponent["command"] = GameEvents.NoOp;
            message.Opponent["args"] = new Array();
            return message;
        }
    }
}