﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using CardGame.Client.Cards;
using Godot;
//using Godot.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using File = System.IO.File;

namespace CardGame.Client
{
    
    public class Library
    {
        private readonly ReadOnlyDictionary<SetCodes, CardInfo> Cards; // = Load();

        public Library()
        {
            Cards = Load();
        }
        public Card Fetch(int id, SetCodes setCode)
        {
            return Create(id, Cards[setCode]);
        }

        
        private Card Create(int id, CardInfo data)
        {
            var cardScene = (PackedScene) ResourceLoader.Load("res://Client/Cards/Card.tscn");
            var card = (Card) cardScene.Instance();
            card.Id = id;
            card.SetData(data);
            return card;
        }

        private ReadOnlyDictionary<SetCodes, CardInfo> Load()
        {
            var cards = JObject.Parse(File.ReadAllText(@"Client\Library.json"));
            var temp = new Dictionary<SetCodes, CardInfo>();
            foreach (var card in cards)
            {
                var source = card.Value;
                var cardType =  (CardTypes) (int) source["Type"];
                var title = (string) source["Title"];
                var art = $"res://Assets/CardArt/{source["Art"]}.png";
                var text = (string) source["Text"];
                var attack = (int) source["Attack"];
                var defense = (int) source["Defense"];
                var cardInfo = new CardInfo(cardType, title, art, text, attack, defense);
                var setCode = (SetCodes) card.Key.ToInt();
                temp.Add(setCode, cardInfo);
            }
            return new ReadOnlyDictionary<SetCodes, CardInfo>(temp);
        }

    }
}
