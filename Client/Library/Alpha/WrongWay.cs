using CardGame.Client.Library.Cards;

namespace CardGame.Client.Library.Alpha
{
    public class WrongWay: Support
    {
        public WrongWay()
        {
            Title = "Wrong Way";
            SetCode = SetCodes.AlphaWrongWay;
            Illustration = "res://Assets/CardArt/sign.png";
            Text = "Return Target Unit To Its Owners Hand";
        }
    }
}