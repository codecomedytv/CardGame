using System.Collections.Generic;
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
        }

        public void Remove(ICardView cardView)
        {
            Cards.Remove(cardView);
        }

        public void Sort()
        {
            throw new System.NotImplementedException();
        }
    }
}