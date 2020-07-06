using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Library.Cards;
using Godot;

namespace CardGame.Client.Room
{
    public class Zone: Object, IEnumerable<Card>
    {
        private readonly Container Container;
        public int Count => Container.GetChildCount();
        public Vector2 Position => Container.RectGlobalPosition;
        public Zone(Container container) => Container = container;
        public Card this[int index] => (Card) Container.GetChild(index);
        public void Add(Card card) => Container.AddChild(card);
        public void Remove(Card card) => Container.RemoveChild(card);
        public void Move(Card card, int index) => Container.MoveChild(card, index);
        public void Sort() => Container.Notification(Container.NotificationSortChildren);

        public IEnumerator<Card> GetEnumerator()
        {
            return (from Card card in Container.GetChildren() select card).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}