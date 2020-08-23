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
            //card3D.Position = new Vector3(card3D.Position.x + 0.5F + card3D.Scale.x * Cards.Count, card3D.Position.y , card3D.Position.z);
            //Transform.Translated(-card3D.Translation);
            //var t = Transform;
            //t.origin.x -= card3D.Scale.x;
            //Transform = t;
            // card3D.Position = new Vector3(card3D.Position.x + 0.5F + card3D.Scale.x * Cards.Count, card3D.Position.y , card3D.Position.z);
        }
        
        // public Vector3 Position
        // { 
        //     get => GetNode<Spatial>("3DCardView").Translation;
        //     set => GetNode<Spatial>("3DCardView").Translation = value;
        // }

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