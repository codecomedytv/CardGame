using System.Collections;
using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Zones
{
    public class Hand: Spatial, IZoneView
    {
        private readonly IList<Card> Cards = new List<Card>();

        public int Count => Cards.Count;
        public void Add(Card card)
        {
            Cards.Add(card);
            card.Position = GlobalTransform.origin;
            Sort();;
        }
        
        public void Remove(Card card)
        {
            Cards.Remove(card);
        }

        public void Sort()
        {
            var card3D = Cards[0];
            var scaleX = card3D.Scale.x;
            Translation = new Vector3(Translation.x - (scaleX / 2.0F), Translation.y, Translation.z);
            var i = 0;
            foreach (var card in Cards)
            {
                // Not entirely sure what I'm doing here other than essentially guessing the new position of cards
                card.Position = GlobalTransform.origin - new Vector3(-0.5F, 0, 0);
                var xMod = card.Position.x + card.Scale.x * i;
                var xMod2 = xMod + 0.2F * i;
                
                card.Position = new Vector3(xMod2, card.Position.y, card.Position.z);
                i += 1;
            }
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return Cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}