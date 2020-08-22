using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Cards.Card3D;
using Godot;

namespace CardGame.Client.Game.Zones
{
    public class Deck: Sprite3D, IZoneView
    {
        private readonly List<ICardView> Cards = new List<ICardView>();
        
        public void Add(ICardView cardView)
        {
            GD.Print("Adding Card View");
            Cards.Add(cardView);
            AddChild((Node) cardView);
            var card3D = (Card3DView) cardView;
            card3D.Position = new Vector3(0, 0, Cards.Count * 0.01F);
        }
        
        public void Remove(ICardView cardView)
        {
            throw new System.NotImplementedException();
        }

        public void Sort()
        {
            throw new System.NotImplementedException();
        }
    }
}