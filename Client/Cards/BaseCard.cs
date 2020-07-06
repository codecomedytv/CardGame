using Godot;

namespace CardGame.Client.Cards
{

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
    
    public class NullCard: BaseCard
    {
        public CardTypes CardType = CardTypes.Null;
        public NullCard()
        {
            SetCode = SetCodes.NullCard;
        }
    }
}