using CardGame.Client.Library.Card;

namespace CardGame.Client.Library.Alpha
{
    public class QuestReward: Support
    {
        public QuestReward()
        {
            Title = "Quest Reward";
            SetCode = SetCodes.Alpha_QuestReward;
            Illustration = "res://Assets/CardArt/coin.png";
            Text = "Draw 2 Cards";
        }
    }
}
