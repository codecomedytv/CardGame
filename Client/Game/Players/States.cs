﻿using System.Diagnostics.Contracts;

namespace CardGame.Client.Game.Players
{
    public enum States
    {
        // States shared by Server & Client
        Acting,
        Active,
        Idle,
        Passing,
        Passive,
        
        // ClientSide-Only States
        Targeting,
        Processing,
        GameOver
    }
}