using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Network;

namespace CardGame.Server.Game.Events
{
    public class SetFaceDown: Event
    {
        public readonly ISource Source;
        public readonly Player Controller;
        public readonly Card Card;

        public SetFaceDown(ISource source, Player controller, Card card)
        {
            Identity = GameEvents.SetFaceDown;
            Source = source;
            Controller = controller;
            Card = card;
        }

        public override void SendMessage(Message message)
        {
            message(Controller.Id, "QueueSetFaceDown", Card.Id);
            message(Controller.Opponent.Id, "QueueSetFaceDown");
        }
    }
}