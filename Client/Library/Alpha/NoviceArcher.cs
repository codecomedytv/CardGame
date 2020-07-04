using CardGame.Client.Library.Cards;

namespace CardGame.Client.Library.Alpha
{
    public class NoviceArcher: Unit
    {
        public NoviceArcher()
        {
            Title = "Novice Archer";
            SetCode = SetCodes.AlphaNoviceArcher;
            Attack = 1000;
            Defense = 1000;
            Text = "When this card is played: Destroy target unit with 1000 or less attack";
            Illustration = "res://Assets/CardArt/M_bot.png";
        }
    }
}
