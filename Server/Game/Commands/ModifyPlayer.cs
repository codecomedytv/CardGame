﻿namespace CardGame.Server.Game.Commands
{
    public class ModifyPlayer: Event
    {
        public readonly Player Player;
        public readonly ISource Source;
        public readonly string Property;
        public readonly object Old;
        public readonly object New;
        
        public ModifyPlayer(GameEvents identity, ISource source, Player player, string property, object oldValue, object newValue)
        {
            GameEvent = identity;
            Source = source;
            Player = player;
            Property = property;
            Old = oldValue;
            New = newValue;
        }

        public override void Execute()
        {
        }


        public override void Undo()
        {
        }
    }
}