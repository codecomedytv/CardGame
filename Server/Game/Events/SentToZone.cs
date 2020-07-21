using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Zones;

namespace CardGame.Server.Game.Events
{

    public class SentToZone : Event
    {
        public readonly Player Controller;
        public readonly Card Card;
        public readonly ZoneIds ZoneId;

        public SentToZone(Player controller, Card card, ZoneIds zoneId)
        {
            Controller = controller;
            Card = card;
            ZoneId = zoneId;
        }

        public override void SendMessage(Message message)
        {
            message(Card.Controller.Id, "SendCardToZone", Card.Id, ZoneId, !IsOpponent);
            message(Card.Controller.Opponent.Id, "SendCardToZone", Card.Id, ZoneId, IsOpponent);
        }
    }
}