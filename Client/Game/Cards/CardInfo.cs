using Newtonsoft.Json;

namespace CardGame.Client.Game.Cards
{
    public enum CardFace
    {
        FaceDown,
        FaceUp,
    }
    
    public enum CardType
    {
        Null,
        Unit,
        Support,
    }
    public readonly struct CardInfo
    {
        public readonly CardType CardType;
        public readonly string Title;
        public readonly string Art;
        public readonly string Text;
        public readonly int Power;

        [JsonConstructor]
        public CardInfo(CardType cardType, string title, string art, string text, int power)
        {
            CardType = cardType;
            Title = title;
            Art = art;
            Text = text;
            Power = power;
        }
    }
}