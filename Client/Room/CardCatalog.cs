using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Cards;
using Godot;

namespace CardGame.Client.Room
{
    public class CardCatalog: Object, IEnumerable<Card>
    {
        [Signal]
        public delegate void CardCreated();
        private readonly Library Library = new Library();
        private readonly Dictionary<int, Card> CardsById = new Dictionary<int, Card>();

        public Card Fetch(int id, SetCodes setCode = SetCodes.NullCard)
        {
            if (id == 0)
            {
                var hidden = Library.Fetch(0, SetCodes.NullCard);
                return hidden;
            }
            if (!CardsById.Keys.Contains(id))
            {
                // Create Card ???
                var card = Library.Fetch(id, setCode);
                RegisterCard(card);
            }
            return CardsById[id];
        }
        
        private Card RegisterCard(Card card)
        {
            CardsById[card.Id] = card;
            EmitSignal(nameof(CardCreated), card);
            return card;
        }
        
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
