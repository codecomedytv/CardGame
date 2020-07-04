using System;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Network;

namespace CardGame.Server.Game.Events
{
    public class Draw: Event
    {
        public readonly ISource Source;
        public readonly Card Card;
        public readonly Player Controller;

        public Draw(ISource source, Player controller, Card card)
        {
            Identity = GameEvents.Draw;
            Source = source;
            Controller = controller;
            Card = card;
        }

        public override void SendMessage(Message message)
        {
            message(Controller.Opponent.Id, "Draw", 0, IsOpponent);
            message(Controller.Id, "Draw", Card.Id, !IsOpponent);
        }
    }
}
