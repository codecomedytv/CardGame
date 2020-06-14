﻿using CardGame.Server.Game.Cards;
using Godot;

namespace CardGame.Server.Game.Network.Messages
{
    public class SetAsDeployable: Message
    {
        public SetAsDeployable(Card unit)
        {
            Player[Command] = (int) GameEvents.SetDeployable;
            Player[Id] = unit.Id;
        }
    }
}
