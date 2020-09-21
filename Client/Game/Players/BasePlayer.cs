using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game.Players
{
    public abstract class BasePlayer: Spatial, IPlayerView
    {
        public abstract int Health { get; set; }
        public abstract Zone Deck { get; protected set; }
        public abstract Zone Graveyard { get; protected set; }
        public abstract Zone Hand { get; protected set; }
        public abstract Zone Units { get; protected set; }
        public abstract Zone Support { get; protected set; }
    }
}