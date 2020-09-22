using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game.Commands
{
    public class LoadDeck: Command
    {
        private readonly Player Player;
        private readonly CardFactory CardFactory;
        private readonly IDictionary<int, SetCodes> DeckList;

        public LoadDeck(Player player, CardFactory cardFactory, IDictionary<int, SetCodes> deckList)
        {
            Player = player;
            CardFactory = cardFactory;
            DeckList = deckList;
            
            // This command needs to execute immediately so other cards exist for incoming commands
            _LoadDeck();
        }
        protected override void SetUp(Effects gfx) { }

        private void _LoadDeck()
        {
            foreach (var (key, value) in DeckList.Select(p => (p.Key, p.Value)))
            {
                var card = CardFactory.Create(key, value);
                card.OwningPlayer = Player;
                card.Controller = Player;
                Player.StateChanged += card.OnPlayerStateChanged;
                Player.Deck.Add(card);
                card.Translation = Player.View.Deck.GlobalTransform.origin;
                card.Translation = new Vector3(card.Translation.x, card.Translation.y, card.ZoneIndex * 0.01F);
            }
        }
    }
}