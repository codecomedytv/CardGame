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
        public readonly int Attack;
        public readonly int Defense;

        [JsonConstructor]
        public CardInfo(CardTypes type, string title, string art, string text, int attack, int defense)
        {
            Type = type;
            Title = title;
            Art = art;
            Text = text;
            Attack = attack;
            Defense = defense;
        }
        
    }
}