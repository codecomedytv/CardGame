using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Zones;

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
            message(Controller.Id, "Deploy", Card.Id, Card.SetCode, !IsOpponent);
            message(Controller.Opponent.Id, "RevealCard", Card.Id, Card.SetCode, ZoneIds.Hand);
            message(Controller.Opponent.Id, "Deploy", Card.Id, Card.SetCode, IsOpponent);
        }
    }
}