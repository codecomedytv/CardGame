using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Zones
{
    public class HandView: Spatial, IZoneView
    {
        private readonly IList<Card> Cards = new List<Card>();

        public int Count => Cards.Count;

        public void Add(Card card)
        {
            Cards.Add(card);
            card.Translation = GlobalTransform.origin;
            var sorter = new Sorter(Cards);
            sorter.Sort();
        }
        
        public void Remove(Card card)
        {
            Cards.Remove(card);
        }

        // public void Sort()
        // {
        //     var sorter = new Sorter(Cards);
        //     sorter.Sort();
        // }

        
    }
}