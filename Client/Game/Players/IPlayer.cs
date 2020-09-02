using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game.Players
{
    public interface IPlayer
    {
        
        Deck Deck { get; }
        Graveyard Graveyard { get;  }
        Hand Hand { get; }
        Units Units { get; }
        Support Support { get; }
        
        // We could probably have this just happen as a response to our life-being set and
        // we wouldn't need to worry about an explicit call via this
        void LoseLife(int lifeLost, Tween gfx);
        
        // Not sure how to handle this but it doesn't look right being here anyway
        void StopDefending();

    }
}