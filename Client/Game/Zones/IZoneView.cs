using CardGame.Client.Game.Cards;

namespace CardGame.Client.Game.Zones
{
    public interface IZoneView
    {
        public void Add(ICardView cardView);
        public void Remove(ICardView cardView);
        public void Sort();
    }
}