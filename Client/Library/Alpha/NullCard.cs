using CardGame.Client.Library.Cards;

namespace CardGame.Client.Library.Alpha
{
    public class NullCard: BaseCard
    {
        public NullCard()
        {
            SetCode = SetCodes.NullCard;
            Illustration = "res://Assets/UI/CardBack_Wooden.png";
        }
    }
}