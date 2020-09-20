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
            Sort();
        }
        
        public void Remove(Card card)
        {
            Cards.Remove(card);
        }

        public void Sort()
        {
            var sorter = new Sorter(Cards);
            sorter.Sort();
        }

        public class Sorter : Godot.Object
        {
            private readonly IList<Card> Cards;
            
            public Sorter(IList<Card> cards)
            {
                Cards = cards;
            }

            public void Sort()
            {
                const float modifier = 1.2F;
                var lastCardOnTheLeft = Cards.Count / 2;
                var initial = -lastCardOnTheLeft * modifier + 0.6F;
                foreach (var card in Cards)
                {
                    card.Translation = new Vector3(initial + 2.5F, card.Translation.y, card.Translation.z);
                    initial += modifier;
                }
            }
            
            
        }
    }
}