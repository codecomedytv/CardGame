using System.Collections;
using System.Collections.Generic;
using System.IO;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Zones
{
    public class Zone: IEnumerable<Card>
    {
        private readonly IList<Card> Cards = new List<Card>();
        public int Count => Cards.Count;

        public Zone() { }

        public Card this[int idx] => Cards[idx];
        public void Add(Card card)
        {
            if (Cards.Contains(card))
            {
                throw new InvalidDataException("Attempted to add a card that already existed in DeckModel");
            }
            Cards.Add(card);
            card.Zone = this;
            card.ZoneIndex = Cards.Count;
        }

        public void Remove(Card card)
        {
            Cards.Remove(card);
        }
        
        public IEnumerator<Card> GetEnumerator() => Cards.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}