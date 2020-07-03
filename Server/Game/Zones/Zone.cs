using System.Collections;
using System.Collections.Generic;
using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Zones
{
    public class Zone : IEnumerable<Card>
    {
        public readonly ZoneIds ZoneId;
        private readonly Player Owner;
        public int Count => Cards.Count;
        public bool IsEmpty => Cards.Count == 0;
        private readonly List<Card> Cards = new List<Card>();
        public Card Top => Cards[Cards.Count - 1];

        public Zone(Player owner, ZoneIds zoneId)
        {
            Owner = owner;
            ZoneId = zoneId;
        }

    public void Add(Card card) => Cards.Add(card);
        public void Remove(Card card) => Cards.Remove(card);
        public bool Contains(Card card) => Cards.Contains(card);
        public void Clear() => Cards.Clear();
        public IEnumerator<Card> GetEnumerator() => Cards.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Card this[int i] => Cards[i];
    }
}
