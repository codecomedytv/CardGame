namespace CardGame.Client.Cards
{
    public enum CardTypes { Null, Unit, Support };
    public enum CardStates { Passive, CanBeDeployed, CanBeSet, CanBeActivated, CanAttack, Activated }

    public readonly struct CardInfo
    {
        public readonly CardTypes CardType;
        public readonly string Title;
        public readonly string Art;
        public readonly string Text;
        public readonly int Attack;
        public readonly int Defense;
        
        public CardInfo(CardTypes cardType, string title, string art, string text, int attack, int defense)
        {
            CardType = cardType;
            Title = title;
            Art = art;
            Text = text;
            Attack = attack;
            Defense = defense;
        }
    }
}