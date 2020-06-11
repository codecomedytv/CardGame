﻿using Godot.Collections;

namespace CardGame.Server.Game.Network.Messages
{
    public class Message
    {
        protected const string Command = "command";
        public Dictionary<string, int> Player = new Dictionary<string, int> {{Command, (int) GameEvents.NoOp}};
        public Dictionary<string, int> Opponent = new Dictionary<string, int>{{Command, (int) GameEvents.NoOp}};

        public Message()
        {
            
        }
    }
}