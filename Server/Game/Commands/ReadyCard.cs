using Godot.Collections;

namespace CardGame.Server.Commands
{
    public class ReadyCard : GameEvent, ICommand
    {
        public readonly Card Card;

        public ReadyCard(Card card)
        {
            Card = card;
        }

        public void Execute()
        {
            Card.Ready = true;
        }

        public void Undo()
        {
            Card.Ready = false;
        }


        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.ReadyCard;
            message.Player["args"] = new Array {Card.Id};
            message.Opponent["command"] = GameEvents.NoOp;
            message.Opponent["args"] = new Array();
            return message;
        }
    }
}