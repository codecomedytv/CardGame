using System.Collections.Generic;
using CardGame.Client.Game.Cards;

namespace CardGame.Client.Game.Zones
{
    public interface IZoneView //IEnumerable<Card>
    {
        public void Add(Card card);
    }
}