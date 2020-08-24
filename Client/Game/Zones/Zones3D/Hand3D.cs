using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Cards.Card3D;
using Godot;

namespace CardGame.Client.Game.Zones.Zones3D
{
    public class Hand3D: Spatial, IZoneView
    {
        private readonly IList<Card3DView> Cards = new List<Card3DView>();

        public void Add(ICardView cardView)
        {
            _add(cardView as Card3DView);
        }

        private void _add(Card3DView card3DView)
        {
            Cards.Add(card3DView);
            card3DView.Translation = GlobalTransform.origin;
            Sort();
        }
        
        public void Remove(ICardView cardView)
        {
            Cards.Remove(cardView as Card3DView);
        }

        public void Sort()
        {
            var card3D = Cards[0];
            var scaleX = card3D.Scale.x;
            Translation = new Vector3(Translation.x - scaleX / 2, Translation.y, Translation.z);
            var i = 1;
            foreach (var card in Cards)
            {
                card.Translation = GlobalTransform.origin;
                card.Translation = new Vector3(card.Translation.x + card.Scale.x * i, card.Translation.y, card.Translation.z);
                i += 1;
            }
        }
    }
}