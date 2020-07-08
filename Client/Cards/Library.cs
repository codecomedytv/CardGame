using System.Collections.Generic;
using System.Collections.ObjectModel;
using Godot;
using Newtonsoft.Json;
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
            //var cards = File.ReadAllText(@"Client/Cards/Library.json");
            var temp = JsonConvert.DeserializeObject<Dictionary<SetCodes, CardInfo>>(File.ReadAllText(@"Client/Cards/Library.json"));
            return new ReadOnlyDictionary<SetCodes, CardInfo>(temp);
        }

    }
}
