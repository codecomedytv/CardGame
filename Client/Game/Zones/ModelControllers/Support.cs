using System.Collections;
using System.Collections.Generic;
using System.IO;
using CardGame.Client.Game.Cards;

namespace CardGame.Client.Game.Zones.ModelControllers
{
    public class Support: IZone
    {
        private readonly IList<Card> Cards = new List<Card>();
        private readonly IZoneView View; 
        public int Count => Cards.Count;

        public Support(IZoneView view) => View = view;

        public void Add(Card card)
        {
            if (Cards.Contains(card))
            {
                throw new InvalidDataException("Attempted to add a card that already existed in DeckModel");
            }
            Cards.Add(card);
            card.Zone = this;
            card.ZoneIndex = Cards.Count;
            View.Add(card);
        }

        public void Remove(Card card)
        {
            Cards.Remove(card);
        }
        
        public IEnumerator<Card> GetEnumerator() => Cards.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}