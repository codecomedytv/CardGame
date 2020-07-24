using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.IO;

namespace CardGame.Client.Cards
{
    
    public static class Library
    {
        private const string JsonFilePath = @"Client/Library/Library.json";
        private static readonly ReadOnlyDictionary<SetCodes, CardInfo> Cards = 
            JsonConvert.DeserializeObject<ReadOnlyDictionary<SetCodes, CardInfo>>(File.ReadAllText(JsonFilePath));
        
        public static Card Fetch(int id, SetCodes setCode)
        {
            return new Card(id, Cards[setCode]);
        }
    }
}
