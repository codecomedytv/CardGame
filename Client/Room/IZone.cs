using System.Collections.Generic;
using CardGame.Client.Cards;

namespace CardGame.Client.Room
{
    public interface IZone: IEnumerable<Card>
    {
        Card this[int index] { get; }
        int Count { get; }
        void Add(Card card);
        void Remove(Card card);
        void Move(Card card, int index);
        void Sort();
        bool Contains(Card card);
        
    }
}