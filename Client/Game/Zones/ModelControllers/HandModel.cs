using System.Collections;
using System.Collections.Generic;
using System.IO;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Zones.ModelControllers
{
    public class HandModel: IZoneModelController
    {
        private readonly IList<Card> Cards = new List<Card>();
        private readonly IZoneView View; 
        public int Count => Cards.Count;

        public HandModel(IZoneView view) => View = view;

        public void Add(Card card)
        {
            if (Cards.Contains(card))
            {
                throw new InvalidDataException("Attempted to add a card that already existed in HandModel");
            }
            Cards.Add(card);
            View.Add(card);
        }

        public void Remove(Card card)
        {
            Cards.Remove(card);
            View.Remove(card);
        }

        public void Sort()
        {
            var v = (HandView) View;
            v.Sort();
        }
        
        public IEnumerator<Card> GetEnumerator() => Cards.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}