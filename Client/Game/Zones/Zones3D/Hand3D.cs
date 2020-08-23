using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Cards.Card3D;
using Godot;

namespace CardGame.Client.Game.Zones.Zones3D
{
    public class Hand3D: Spatial, IZoneView
    {
        public void Add(ICardView cardView)
        {
            AddChild((Card3DView) cardView);
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