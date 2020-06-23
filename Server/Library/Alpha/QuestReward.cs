using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Skill;

namespace CardGame.Server
{
    public class QuestReward : Support
    {
        public QuestReward()
        {
            Title = "QuestReward";
            SetCode = SetCodes.Alpha_QuestReward;
            AddSkill(new DrawXCards());
        }

        private class DrawXCards : Skill
        {
            protected override void _Resolve()
            {
                Controller.Draw();
                Controller.Draw();
            }
        }
    }
}