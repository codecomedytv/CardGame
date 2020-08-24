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
            Translation = new Vector3(Translation.x - 0.4F, Translation.y, Translation.z);
            var i = 0;
            foreach (var card in Cards)
            {
                // Not entirely sure what I'm doing here other than essentially guessing the new position of cards
                
                // Move Card To Origin (right-most point)
                card.Position = GlobalTransform.origin;

                // 0.1F is so cards don't huddle together, another 0.1F is to give an actual space between cards
                var fixPixelOffByOneMod = i * 0.2F;
                
                // Move the card forwards the line times its position in the list of cards
                var xMod = card.Position.x + i + fixPixelOffByOneMod;
               
                // Finally set the position
                card.Position = new Vector3(xMod, card.Position.y, card.Position.z);
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