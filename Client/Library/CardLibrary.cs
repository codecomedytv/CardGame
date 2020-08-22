using System.Collections.ObjectModel;
using System.IO;
using CardGame.Client.Game.Cards;
using Newtonsoft.Json;

namespace CardGame.Client.Library
{
    
    public static class CardLibrary
    {
        private const string JsonCardsFilePath = @"Client/Library/Library.json";
        public static readonly ReadOnlyDictionary<SetCodes, CardInfo> Cards =
            JsonConvert.DeserializeObject<ReadOnlyDictionary<SetCodes,
                CardInfo>>(File.ReadAllText(JsonCardsFilePath));
    }
    
}