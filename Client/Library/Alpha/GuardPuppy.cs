using CardGame.Client.Library.Card;

namespace CardGame.Client.Library.Alpha
{
    public class GuardPuppy: Unit
    {
        public GuardPuppy()
        {
            Title = "Guard Puppy";
            Text = "Cannot Be Destroyed By Battle.\nYour opponent cannot attack other units";
            SetCode = SetCodes.Alpha_GuardPuppy;
            Attack = 500;
            Defense = 500;
            Illustration = "res://Assets/CardArt/boar.png";
        }
    }
}
