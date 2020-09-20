using System.Collections.Generic;
using CardGame.Client.Game.Cards;

namespace CardGame.Client.Game.Zones.ModelControllers
{
    public interface IZoneModelController: IEnumerable<Card>
    {
        public IZone View { get; set; } // Do we need this really?
        public int Count { get; }

        public void Add(Card card);
        public void Remove(Card card);
    }
}