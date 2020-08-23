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

        public void Add(ICardView cardView)
        {
            Cards.Add(cardView);
            var card3D = (Card3DView) cardView;
            card3D.Translation = GlobalTransform.origin;
            //card3D.GlobalTransform.origin = GlobalTransform.origin;
            //card3D.GlobalTransform = GlobalTransform;
            Sort();
        }
        
        public void Remove(ICardView cardView)
        {
            Cards.Remove(cardView);
        }

        public void Sort()
        {
            // Need to look into a way to avoid all the casting
            var card3D = (Card3DView) Cards[0];
            var scaleX = card3D.Scale.x;
            Translation = new Vector3(Translation.x - scaleX / 2, Translation.y, Translation.z);
            var i = 1;
            foreach (var cardView in Cards)
            {
                var card = (Card3DView) cardView;
                card.Translation = GlobalTransform.origin;
                card.Translation = new Vector3(card.Translation.x + card.Scale.x * i, card.Translation.y, card.Translation.z);
                i += 1;
            }
        }
    }
}