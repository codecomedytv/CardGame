using System.Collections.Generic;
using CardGame.Client.Game.Cards;

namespace CardGame.Client.Game.Zones
{
    public interface IZoneView //IEnumerable<Card>
    {
        public int Count { get; }
        public void Add(Card card);
        public void Remove(Card card);
    }
}