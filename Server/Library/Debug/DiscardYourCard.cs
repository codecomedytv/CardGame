using CardGame.Server.Room.Cards;
using CardGame.Server.Room.Commands;

namespace CardGame.Server
{
    public class DiscardYourCard : Support
    {
        public DiscardYourCard()
        {
            Title = "Debug.DiscardYourCard";
            SetCode = SetCodes.DebugDiscardYourCard;
            AddSkill(new DiscardCard());
        }

        private class DiscardCard : Skill
        {
            public override void _SetUp()
            {
                SetTargets(Controller.Hand);
                CanBeUsed = Controller.Hand.Count > 0;
            }

            protected override void _Resolve()
            {
                Controller.DeclarePlay(new Move(Card, Controller, Target, Target.Owner.Graveyard));
            }
        }
    }
}