using System.Collections.Generic;
using System.Collections.ObjectModel;
using Godot;
using Newtonsoft.Json.Linq;
using File = System.IO.File;

namespace CardGame.Client.Cards
{
    
    public static class Library
    {
        private static readonly ReadOnlyDictionary<SetCodes, CardInfo> Cards = Load();
        
        public static Card Fetch(int id, SetCodes setCode)
        {
            return new Card(id, Cards[setCode]);
        }
        
        private static ReadOnlyDictionary<SetCodes, CardInfo> Load()
        {
            var cards = JObject.Parse(File.ReadAllText(@"Client/Cards/Library.json"));
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
