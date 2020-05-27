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

        public class DrawXCards : Skill
        {
            public override void _Resolve()
            {
                Controller.Draw(2);
            }
        }
    }
}