using System.Collections.Generic;
using CardGame.Client.Game.Cards;

namespace CardGame.Client.Game.Zones
{
    public interface IZoneView: IEnumerable<ICardView>
    {
        public void Add(ICardView cardView);
        public void Remove(ICardView cardView);
        public void Sort();
    }
}