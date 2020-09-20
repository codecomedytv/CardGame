using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Zones
{
    public class HandView : Spatial, IZoneView
    {
        public void Add(Card card)
        {
            card.Translation = GlobalTransform.origin;
        }
    }
}