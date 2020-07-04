using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Library;
using CardGame.Client.Library.Cards;
using Godot;
using Array = Godot.Collections.Array;
using Object = Godot.Object;

namespace CardGame.Client.Room
{
    public class CardCatalog: Object
    {
        [Signal]
        public delegate void CardCreated();
        
        private readonly System.Collections.Generic.Dictionary<int, Card> CardsById = new System.Collections.Generic.Dictionary<int, Card>();
        private List<Card> HiddenCards = new List<Card>();
        
        private void RegisterCard(Card card)
        {
            CardsById[card.Id] = card;
            EmitSignal(nameof(CardCreated), card);
        }

        public Card this[int id]
        {
            get => CardsById[id] = GetCard(id);
            set => CardsById[id] = AddCard(value);
        }
        
        private Card GetCard(int id)
        {
            if (id != 0) return CardsById[id];
            var hidden = CheckOut.Fetch(0, SetCodes.NullCard);
            HiddenCards.Add(hidden);
            return hidden;
        }
        
        private Card AddCard(Card card)
        {
            RegisterCard(card);
            return card;
        }

        
    }
}
