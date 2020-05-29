using CardGame.Client.Library.Card;

namespace CardGame.Client.Library.Alpha
{
    public class CounterAttack: Support
    {
        public CounterAttack()
        {
            Title = "Counter Attack";
            SetCode = SetCodes.Alpha_CounterAttack;
            Illustration = "res://Assets/CardArt/meat.png";
            Text = "Destroy Attacking Monster";
        }

    }
}