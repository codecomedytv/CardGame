﻿using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Game.Cards;
using Godot;

namespace CardGame.Server.Game.Events
{
    public class SelectTarget: Event
    {
        // This isn't really a game even we're just using it to be consistent with everything else
        public readonly ISource Source;
        public readonly Player Player;
        public readonly List<Card> Targets;

        public SelectTarget(ISource source, Player player, List<Card> validTargets)
        {
            Source = source;
            Player = player;
            Targets = validTargets;
            GD.Print($"Valid Targets Count is {validTargets.Count} (Source SelectTargetEvent)");

        }
    }
}