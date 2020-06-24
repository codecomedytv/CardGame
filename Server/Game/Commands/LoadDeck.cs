using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Commands
{
    public class LoadDeck : Command
    {
	    public readonly GameEvents Identity = GameEvents.LoadDeck;
        public readonly ISource Source;
        public readonly Player Player;
        public readonly List<SetCodes> DeckList;
        public readonly ReadOnlyCollection<Card> CardsLoaded;
        public readonly Match Match;
        private ReadOnlyCollection<Card> CardsAlreadyLoaded;

        public LoadDeck()
        {
            
        }
        public LoadDeck(Player player, Match match)
        {
	        Source = player;
	        Player = player;
	        DeckList = player.DeckList;
            Match = match;
        }

        public override void Execute()
        {
	        if (CardsAlreadyLoaded != null)
	        {
		        Player.Deck.Clear();
		        foreach(var card in CardsAlreadyLoaded) {Player.Deck.Add(card);}

		        return;
	        }

	        foreach (var card in DeckList.Select(setCode => Library.Create(setCode)))
	        {
		        card.Skill.Match = Match;
		        card.Owner = Player;
		        card.Controller = Player;
		        card.Zone = Player.Deck;
		        Match.RegisterCard(card);
		        Player.Deck.Add(card);
	        }

	        CardsAlreadyLoaded = new ReadOnlyCollection<Card>(Player.Deck.ToList());
        }

        public override void Undo()
        {
	        Player.Deck.Clear();
        }
    }
}
