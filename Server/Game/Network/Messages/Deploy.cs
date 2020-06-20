﻿using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Network.Messages
{
    public class Deploy: Message
    {
        public Deploy(Card card)
        {
            Player[Command] = (int) GameEvents.Deploy;
            Player[Id] = card.Id;
            Opponent[Command] = (int) GameEvents.OpponentDeploy;
            Opponent[Id] = card.Id;
            Opponent[SetCode] = (int) card.SetCode;

        }
    }
}