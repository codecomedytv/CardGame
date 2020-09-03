using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game.Players
{
    public abstract class BasePlayer: Spatial
    {
        public abstract int Health { get; set; }
        public abstract Deck Deck { get; protected set; }
        public abstract Graveyard Graveyard { get; protected set; }
        public abstract Hand Hand { get; protected set; }
        public abstract Units Units { get; protected set; }
        public abstract Support Support { get; protected set; }
    }
}