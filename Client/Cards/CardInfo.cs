using Newtonsoft.Json;

namespace CardGame.Client.Cards
{
    public enum CardTypes { Null, Unit, Support };
    public enum CardStates { Passive, CanBeDeployed, CanBeSet, CanBeActivated, CanAttack, Activated }

    public readonly struct CardInfo
    {
        public readonly CardTypes Type;
        public readonly string Title;
        public readonly string Art;
        public readonly string Text;
        public readonly int Power;

        [JsonConstructor]
        public CardInfo(CardTypes type, string title, string art, string text, int power, int defense)
        {
            Type = type;
            Title = title;
            Art = art;
            Text = text;
            Power = power;
        }
        
    }
}