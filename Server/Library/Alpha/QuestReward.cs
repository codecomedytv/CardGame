using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Skills;

namespace CardGame.Server
{
    public class QuestReward : Support
    {
        public QuestReward(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "QuestReward";
            SetCode = SetCodes.Alpha_QuestReward;
            Skill = new DrawXCards(this);
        }

        private class DrawXCards : Manual
        {
            public DrawXCards(Card card)
            {
                Card = card;
                AreaOfEffects.Add(Controller.Support);
            }
            protected override void _Resolve()
            {
                Controller.Draw();
                Controller.Draw();
            }
        }
    }
}