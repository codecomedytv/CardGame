using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Library;
using CardGame.Client.Library.Cards;
using Godot;

namespace CardGame.Client.Room
{
    public class CardCatalog: Object, IEnumerable<Card>
    {
        [Signal]
        public delegate void CardCreated();
        private readonly Dictionary<int, Card> CardsById = new Dictionary<int, Card>();

        public Card Fetch(int id, SetCodes setCode = SetCodes.NullCard)
        {
            if (id == 0)
            {
                var hidden = CheckOut.Fetch(0, SetCodes.NullCard);
                return hidden;
            }
            if (!CardsById.Keys.Contains(id))
            {
                // Create Card ???
                var card = CheckOut.Fetch(id, setCode);
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

        // public Card this[int id]
        // {
        //     get => CardsById[id] = GetCard(id);
        //     set => CardsById[id] = RegisterCard(value);
        // }
        
        
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
