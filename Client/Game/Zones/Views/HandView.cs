using System.Collections;
using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Zones
{
    public class HandView: Spatial, IZoneView
    {
        private readonly IList<Card> Cards = new List<Card>();

        public int Count => Cards.Count;

        public Card this[int index] => Cards[index];
        public void Add(Card card)
        {
            Cards.Add(card);
            card.Translation = GlobalTransform.origin;
            Translation = new Vector3(Translation.x - 0.5F, Translation.y, Translation.z);
            Sort();;
        }
        
        public void Remove(Card card)
        {
            Cards.Remove(card);
            Translation = new Vector3(Translation.x + 0.5F, Translation.y, Translation.z);
        }

        public void Sort()
        {
            var i = 0;
            foreach (var card in Cards)
            {
                // Move Card To Origin (right-most point)
                card.Translation = GlobalTransform.origin;

                // 0.1F is so cards don't huddle together, another 0.2F is to give an actual space between cards
                var fixPixelOffByOneMod = i * 0.3F;
                
                // Move the card forwards the line times its position in the list of cards
                var xMod = card.Translation.x + i + fixPixelOffByOneMod;
               
                // Finally set the position
                card.Translation = new Vector3(xMod, card.Translation.y, card.Translation.z);
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