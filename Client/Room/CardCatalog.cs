using System.Collections.Generic;
using CardGame.Client.Library;
using CardGame.Client.Library.Cards;
using Godot;

namespace CardGame.Client.Room
{
    public class CardCatalog: Object
    {
        [Signal]
        public delegate void CardCreated();
        private readonly Dictionary<int, Card> CardsById = new Dictionary<int, Card>();
        
        private Card RegisterCard(Card card)
        {
            CardsById[card.Id] = card;
            EmitSignal(nameof(CardCreated), card);
            return card;
        }

        public Card this[int id]
        {
            get => CardsById[id] = GetCard(id);
            set => CardsById[id] = RegisterCard(value);
        }
        
        private Card GetCard(int id)
        {
            if (id != 0) return CardsById[id];
            var hidden = CheckOut.Fetch(0, SetCodes.NullCard);
            return hidden;
        }
        
    }
}
