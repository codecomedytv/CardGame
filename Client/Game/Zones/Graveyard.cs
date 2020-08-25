using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Zones
{
    public class Graveyard: Sprite3D, IZone
    {   
        private IList<Card> Cards = new List<Card>();
        public IEnumerator<Card> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get; }
        public void Add(Card card)
        {
            throw new System.NotImplementedException();
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