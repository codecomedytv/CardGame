using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Library.Cards;
using Godot;

namespace CardGame.Client.Room
{
    public class CardCatalog: Object
    {
        [Signal]
        public delegate void DoubleClicked();
        
        private readonly Dictionary<int, Card> CardsById = new Dictionary<int, Card>();
        
        public void RegisterCard(Card card)
        {
            CardsById[card.Id] = card;
        }

        public Card this[int id]
        {
            get => CardsById[id];
            set => CardsById[id] = AddCard(value);
        }
        
        private Card AddCard(Card card)
        {
            card.Connect(nameof(Card.DoubleClicked), this, nameof(this.OnCardDoubleClicked));
            return card;
        }

        private void OnCardDoubleClicked(Card card)
        {
            GD.Print($"{card} was double clicked");
        }
    }
}