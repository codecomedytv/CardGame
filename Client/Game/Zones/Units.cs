using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Zones
{
    public class Units: Spatial, IZone
    {
        // int == ZoneId
        // We're going to do 4, 2, 0, 1, 3
        private IList<Card> Cards = new List<Card>();
        public IEnumerator<Card> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Vector3 NextSlot()
        {
            return GetNode<Sprite3D>($"CardSlot{Count}").GlobalTransform.origin;
        }

        public int Count => Cards.Count;
        
        public void Add(Card card)
        {
            Cards.Add(card);
        }

        public void Remove(Card card)
        {
            throw new System.NotImplementedException();
        }

        public void Sort()
        {
            throw new System.NotImplementedException();
        }
    }
}