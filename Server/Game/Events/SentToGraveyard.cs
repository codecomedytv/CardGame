using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Network;

namespace CardGame.Server.Game.Events
{
    public class SentToGraveyard: Event
    {
        public readonly Player Controller;
        public readonly Card Card;

        public SentToGraveyard(Player controller, Card card)
        {
            Controller = controller;
            Card = card;
        }

        public override void SendMessage(Message message)
        {
            message(Controller.Id, "SentToGraveyard", Card.Id);
            message(Controller.Opponent.Id, "SentToGraveyard", Card.Id);
        }
    }
}