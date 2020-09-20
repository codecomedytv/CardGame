﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using CardGame.Client.Game.Cards;

namespace CardGame.Client.Game.Zones.ModelControllers
{
    public class DeckModel: IZoneModelController
    {
        private readonly IList<Card> Cards = new List<Card>();
        public IZone View { get; set; }
        public int Count => Cards.Count;

        public DeckModel(IZone view) => View = view;

        public void Add(Card card)
        {
            if (Cards.Contains(card))
            {
                throw new InvalidDataException("Attempted to add a card that already existed in DeckModel");
            }
            Cards.Add(card);
            View.Add(card);
        }

        public void Remove(Card card)
        {
            Cards.Remove(card);
            View.Remove(card);
        }

        public IEnumerator<Card> GetEnumerator() => Cards.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}