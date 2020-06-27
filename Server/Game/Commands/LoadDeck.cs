using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Commands
{
    public class LoadDeck : Event
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly IReadOnlyCollection<Card> Deck;
        private ReadOnlyCollection<Card> CardsAlreadyLoaded;

        public LoadDeck()
        {
            
        }
        public LoadDeck(Player player, IReadOnlyCollection<Card> deck)
        {
	        GameEvent = GameEvents.LoadDeck;
	        Source = player;
	        Player = player;
	        Deck = deck;
        }

        public override void Execute()
        {

        }

        public override void Undo()
        {
        }
    }
}
