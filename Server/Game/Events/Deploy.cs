using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Network;

namespace CardGame.Server.Game.Events
{
    public class Deploy: Event
    {
        public readonly ISource Source;
        public readonly Card Card;
        public readonly Player Controller;

        public Deploy(ISource source, Player controller, Card card)
        {
            Identity = GameEvents.Deploy;
            Source = source;
            Card = card;
            Controller = controller;
        }

        public override void SendMessage(Message message)
        {
            message(Controller.Id, "QueueDeploy", Card.Id, Card.SetCode, !IsOpponent);
            message(Controller.Opponent.Id, "QueueDeploy", Card.Id, Card.SetCode, IsOpponent);
        }
    }
}