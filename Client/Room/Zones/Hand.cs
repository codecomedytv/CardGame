using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Cards;
using Godot;

namespace CardGame.Client.Room.Zones
{
    public class Hand: Object, IZone
    {
        private readonly Spatial Container;
        public int Count => Container.GetChildCount();
        public Vector3 Position => Container.GlobalTransform.origin;
        
        public Hand(Spatial container) => Container = container;
        
        public Card this[int index] => (Card) Container.GetChild(index);

        public void Add(Card card)
        { 
            Container.AddChild(card);
            card.Zone = this;
        }

        public void Remove(Card card) => Container.RemoveChild(card);
        public void Move(Card card, int index) => Container.MoveChild(card, index);
        public void Sort() { GD.PushWarning("Sorting Requires Reimplementation"); }

        public bool Contains(Card card) => Container.GetChildren().Contains(card);

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