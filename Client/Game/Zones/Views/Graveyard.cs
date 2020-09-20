﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Zones
{
    public class Graveyard: Sprite3D, IZone
    {   
        private IList<Card> Cards = new List<Card>();
        public IEnumerator<Card> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => Cards.Count;
        public void Add(Card card)
        {
            Cards.Add(card);
            card.Translation = GlobalTransform.origin;
        }

        public void Remove(Card card)
        {
            Cards.Remove(card);
        }
        
        public void Sort()
        {
            throw new System.NotImplementedException();
        }
    }
}