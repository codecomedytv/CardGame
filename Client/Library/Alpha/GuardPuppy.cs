using CardGame.Client.Library.Cards;

namespace CardGame.Client.Library.Alpha
{
    public class GuardPuppy: Unit
    {
        public GuardPuppy()
        {
            Title = "Guard Puppy";
            Text = "Cannot Be Destroyed By Battle.\nYour opponent cannot attack other units";
            SetCode = SetCodes.AlphaGuardPuppy;
            Attack = 500;
            Defense = 500;
            Illustration = "res://Assets/CardArt/boar.png";
        }
    }
}
