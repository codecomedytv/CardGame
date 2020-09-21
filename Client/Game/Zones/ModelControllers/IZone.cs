using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Zones.ModelControllers
{
    public interface IZone: IEnumerable<Card>
    {
        //public readonly Spatial View { get; }
        public int Count { get; }
        public void Add(Card card);
        public void Remove(Card card);
    }
}