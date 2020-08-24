using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Cards.Card3D;
using Godot;

namespace CardGame.Client.Game.Zones.Zones3D
{
    public class Hand3D: Spatial, IZoneView
    {
        private readonly IList<ICardView> Cards = new List<ICardView>();

        public void Add(ICardView card)
        {
            Cards.Add(card);
            card.Position = GlobalTransform.origin;
            Sort();;
        }
        
        public void Remove(ICardView card)
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
    }
}