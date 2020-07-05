using System.Collections;
using System.Collections.Generic;
using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game
{
    public class CardCatalog : IEnumerable<Card>
    {
        // We keep 0 as an identifier for null card clientside
        private int NextCardId = 1;
        private readonly Godot.Collections.Dictionary<int, Card> CardsById = new Godot.Collections.Dictionary<int, Card>();
        
        public void RegisterCard(Card card)
        {
            card.Id = NextCardId;
            CardsById[card.Id] = card;
            NextCardId += 1;
        }

        public Card this[int id] => CardsById[id];
        public IEnumerator<Card> GetEnumerator()
        {
            return CardsById.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}