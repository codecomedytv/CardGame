using CardGame.Client.Library.Cards;

namespace CardGame.Client.Library.Alpha
{
    public class CounterAttack: Support
    {
        public CounterAttack()
        {
            Title = "Counter Attack";
            SetCode = SetCodes.AlphaCounterAttack;
            Illustration = "res://Assets/CardArt/meat.png";
            Text = "Destroy Attacking Monster";
        }

    }
}