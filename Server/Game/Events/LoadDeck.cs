using System.Collections.Generic;
using System.Collections.ObjectModel;
using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Events
{
    public class LoadDeck : Event
    {
	    public const GameEvents GameEvent = GameEvents.LoadDeck;
        public readonly ISource Source;
        public readonly Player Player;
        public readonly IReadOnlyCollection<Card> Deck;

        public LoadDeck()
        {
            
        }
        public LoadDeck(Player player, IReadOnlyCollection<Card> deck)
        {
	        Source = player;
	        Player = player;
	        Deck = deck;
        }


    }
}
