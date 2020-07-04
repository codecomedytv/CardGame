using CardGame.Client.Library.Cards;

namespace CardGame.Client.Library.Alpha
{
    public class QuestReward: Support
    {
        public QuestReward()
        {
            Title = "Quest Reward";
            SetCode = SetCodes.AlphaQuestReward;
            Illustration = "res://Assets/CardArt/coin.png";
            Text = "Draw 2 Cards";
        }
    }
}
