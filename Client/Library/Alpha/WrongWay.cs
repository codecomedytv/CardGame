using CardGame.Client.Library.Card;

namespace CardGame.Client.Library.Alpha
{
    public class WrongWay: Support
    {
        public WrongWay()
        {
            Title = "Wrong Way";
            SetCode = SetCodes.Alpha_WrongWay;
            Illustration = "res://Assets/CardArt/sign.png";
            Text = "Return Target Unit To Its Owners Hand";
        }
    }
}