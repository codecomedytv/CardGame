using Godot;

namespace CardGame.Client.Library.Card
{
    public enum CardTypes
    {
        Unit,
        Support
    };
    public class BaseCard: Reference
    {
        public string Title = "Card";
        public SetCodes SetCode;
        public int Id;
        public Reference Owner;
        public Reference Controller;
        public string Illustration;
        public string Text;
    }

    public class Unit : BaseCard
    {
        public CardTypes CardType = CardTypes.Unit;
        public int Attack = 0;
        public int Defense = 0;
    }

    public class Support : BaseCard
    {
        public CardTypes CardType = CardTypes.Support;
    }
}