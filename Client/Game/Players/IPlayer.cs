using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game.Players
{
    public interface IPlayer
    {
        int Health { get; set; }
        Deck Deck { get; }
        Graveyard Graveyard { get;  }
        Hand Hand { get; }
        Units Units { get; }
        Support Support { get; }
      
        // Not sure how to handle this but it doesn't look right being here anyway
        void StopDefending();

    }
}