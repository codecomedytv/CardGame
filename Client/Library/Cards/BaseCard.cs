using Godot;

namespace CardGame.Client.Library.Cards
{
    public enum CardTypes
    {
        Null,
        Unit,
        Support
    };
    public class BaseCard: Reference
    {
        public string Title = "Card";
        protected SetCodes SetCode;
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
    
    public class NullCard: BaseCard
    {
        public CardTypes CardType = CardTypes.Null;
        public NullCard()
        {
            SetCode = SetCodes.NullCard;
        }
    }
}