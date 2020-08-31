using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public override void SendMessage(Message message)
        {
	        message(Player.Id, Commands.LoadDeck, Deck.ToDictionary(c => c.Id, c => c.SetCode));
        }


    }
}
